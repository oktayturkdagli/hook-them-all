using System.Collections;
using UnityEngine;
using DG.Tweening;

//I Implemented this, from https://www.youtube.com/watch?v=Qxs3GrhcZI8
public class Projectile3D : MonoBehaviour
{
    [SerializeField] public bool canUse = true, canShoot = true, isShooted = false;
    [SerializeField] private LineRenderer _Line;
    [SerializeField] private float _Step;
    [SerializeField] private Transform _Firepoint;
    [SerializeField] private Transform _Aim;
    [SerializeField] private LayerMask maskLayer;
    private Vector3 defaultPosition = Vector3.zero;
    private Quaternion defaultRotation = Quaternion.identity;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        HandleShoot();
    }

    void HandleShoot()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue, maskLayer) && canUse)
        {
            if (!_Line.gameObject.activeSelf && canShoot)
                _Line.gameObject.SetActive(true);
            
            Vector3 direction = hit.point - _Firepoint.position;
            Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
            Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
            float height = targetPos.y + targetPos.magnitude / 2f;
            height = Mathf.Max(0.01f, height);
            float angle;
            float v0;
            float time;
            CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);
            DrawPath(groundDirection.normalized, v0, angle, time, _Step);
            if (isShooted && canShoot)
            {
                isShooted = false;
                canShoot = false;
                _Line.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time));
            }
        }
        else
        {
            if (_Line.gameObject.activeSelf)
                _Line.gameObject.SetActive(false);
            isShooted = false;
        }
    }
    
    void PullBack(Transform pullBackWithThis)
    {
        transform.DOMove(_Firepoint.transform.position, 2f).SetUpdate(UpdateType.Fixed).OnComplete(Reset);
        pullBackWithThis.DOMove(_Firepoint.transform.position, 2f).SetUpdate(UpdateType.Fixed);
    }

    void Reset()
    {
        canShoot = true;
        _Line.gameObject.SetActive(true);
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }
    
    void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time/step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _Firepoint.position + direction * x + Vector3.up * y);
            count++;
        }
        float xfinal = v0 * time * Mathf.Cos(angle);
        float yfinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, _Firepoint.position + direction * xfinal + Vector3.up * yfinal);
        _Aim.transform.position = _Firepoint.position + direction * xfinal + Vector3.up * yfinal;
    }

    float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;
        angle = Mathf.Atan(b * time) / xt;

        v0 = b / Mathf.Sin(angle);
    }
    
    void RotateTowards(Vector3 to) 
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = lookRotation; //instant
        // transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * timeMultiplier); //over time
    }
    
    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time)
    {
        float t = 0;
        while (t<time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            Vector3 rotationVector = _Firepoint.position + direction * x + Vector3.up * y;
            RotateTowards(rotationVector);
            transform.position = _Firepoint.position + direction * x + Vector3.up * y;
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            StopAllCoroutines();
            PullBack(collision.transform);
        }
    }
    
}

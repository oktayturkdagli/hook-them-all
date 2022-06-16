using System;
using System.Collections;
using UnityEngine;

//I Implemented this, https://www.youtube.com/watch?v=Qxs3GrhcZI8
public class Projectile2D : MonoBehaviour
{
    [SerializeField] private float _InitialVelocity;
    [SerializeField] private float _Angle;
    [SerializeField] private LineRenderer _Line;
    [SerializeField] private float _Step;
    [SerializeField] private Transform _Firepoint;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Vector3 targetPos = _cam.ScreenToWorldPoint(Input.mousePosition) - _Firepoint.position;
        targetPos.z = 0;
        float height = targetPos.y + targetPos.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        float angle;
        float v0;
        float time;
        CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);

        DrawPath(v0, angle, time, _Step);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(v0, angle, time));
        }
    }

    void DrawPath(float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time/step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _Firepoint.position + new Vector3(x,y,0));
            count++;
        }
        float xfinal = v0 * time * Mathf.Cos(angle);
        float yfinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, _Firepoint.position + new Vector3(xfinal,yfinal,0));
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
    
    void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2-v3));
        
        time = xt / (v0 * Mathf.Cos(angle));
    }

    IEnumerator Coroutine_Movement(float v0, float angle, float time)
    {
        float t = 0;
        while (t<time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _Firepoint.position + new Vector3(x, y, 0);
            
            t += Time.deltaTime;
            yield return null;
        }
    }
}

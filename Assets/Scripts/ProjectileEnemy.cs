using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

//I Implemented this, https://www.youtube.com/watch?v=Qxs3GrhcZI8
public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private bool canShoot = true;
    private Vector3 defaultPosition = Vector3.zero;
    private Quaternion defaultRotation = Quaternion.identity;
    [SerializeField] private Transform _Firepoint;
    
    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        HandleShoot();
    }
    
    void HandleShoot()
    {
        if (canShoot)
        {
            Reset();
            canShoot = false;
            Vector3 destination = new Vector3(Random.Range(-15f, 15f), 2.5f, Random.Range(-1.5f, 1.5f));
            transform.DOJump(destination, 1f, 1, 2f).OnComplete(() =>
            {
                Reset();
                HandleShoot();
            });
        }
    }
    
    void PullBack(Transform pullBackWithThis)
    {
        transform.DOMove(_Firepoint.transform.position, 2f).SetUpdate(UpdateType.Fixed).OnComplete(() =>
        {
            Reset();
            HandleShoot();
        });
        pullBackWithThis.DOMove(_Firepoint.transform.position, 2f).SetUpdate(UpdateType.Fixed);
    }

    private void Reset()
    {
        canShoot = true;
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ally"))
        {
            transform.DOKill();
            PullBack(collision.transform);
        }
    }
    
}

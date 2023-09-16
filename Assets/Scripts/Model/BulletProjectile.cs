using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    private Vector3 _targetPosition;
    private float _speed = 200f;
    
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update() 
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        transform.Translate(moveDir * Time.deltaTime * _speed);

        if(Vector3.Distance(transform.position, _targetPosition) <= 1f)
        {
            trailRenderer.transform.parent = null;
            Instantiate(bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

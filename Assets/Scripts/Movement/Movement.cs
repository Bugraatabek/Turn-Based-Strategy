using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IUnit
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 10f;
    
    private Vector3 targetPosition;
    private bool isMoving = false;
    public event Action<bool> animatorHandlerWalk;

    public void TryInvokeMovement(Vector3 targetPosition)
    {
        if(isMoving)
        {
            print("Unit is already moving");
            return;
        } 
        isMoving = true;
        this.targetPosition = targetPosition;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        animatorHandlerWalk?.Invoke(true);
        float stoppingDistance = 0.1f;
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= stoppingDistance) 
            {
                isMoving = false;
                animatorHandlerWalk?.Invoke(false);
                yield break;
            }
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            RotateUnit(moveDirection);
            
            yield return null;
        }
    }

    private void RotateUnit(Vector3 rotateDirection)
    {
        transform.forward = Vector3.Lerp(transform.forward, rotateDirection, Time.deltaTime * _rotationSpeed);
    }
}

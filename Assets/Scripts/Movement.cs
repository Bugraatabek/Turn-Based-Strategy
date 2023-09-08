using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float _moveSpeed = 4f;
    private bool isMoving = false;

    private void OnEnable() 
    {
        InputReader.movementTryInvokeMovement += TryInvokeMovement;
    }

    private void OnDisable() 
    {
        InputReader.movementTryInvokeMovement -= TryInvokeMovement;
    }

    private void TryInvokeMovement(Vector3 targetPosition)
    {
        if(isMoving) return;
        isMoving = true;
        this.targetPosition = targetPosition;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float stoppingDistance = 0.1f;
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= stoppingDistance) 
            {
                isMoving = false;
                yield break;
            }
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}

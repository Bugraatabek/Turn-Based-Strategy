using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private float _moveSpeed = 4f;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4,0,4));
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        StartCoroutine(MoveTowardsTargetPosition());
    }

    private IEnumerator MoveTowardsTargetPosition()
    {
        float stoppingDistance = 0.1f;
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= stoppingDistance) yield break;
            
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            print(Vector3.Distance(transform.position, targetPosition));
            yield return null;
        }
    }
}

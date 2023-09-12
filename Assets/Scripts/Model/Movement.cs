using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 10f;
    
    private Vector3 targetPosition;
    private GridPosition _currentGridPosition;
    private bool isMoving = false;
    public event Action<bool> OnMovingStateChanged;
    public event Action<GridPosition> onGridPositionChanged;


    private void Start() 
    {
        CheckCurrentGridPosition();
    }

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
        OnMovingStateChanged?.Invoke(true);
        float stoppingDistance = 0.1f;
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= stoppingDistance) 
            {
                isMoving = false;
                OnMovingStateChanged?.Invoke(false);
                CheckCurrentGridPosition();
                yield break;
            }
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            RotateUnit(moveDirection);
            CheckCurrentGridPosition();
            yield return null;
        }
    }

    private void RotateUnit(Vector3 rotateDirection)
    {
        transform.forward = Vector3.Lerp(transform.forward, rotateDirection, Time.deltaTime * _rotationSpeed);
    }

    private void CheckCurrentGridPosition()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(_currentGridPosition != gridPosition)
        {
            _currentGridPosition = gridPosition;
            onGridPositionChanged?.Invoke(_currentGridPosition);
        }
    }
}

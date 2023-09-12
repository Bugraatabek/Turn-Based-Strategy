using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private int maxMoveDistance;
    
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
                CheckCurrentGridPosition(); // updates _currentGridPosition after the action finishes.
                yield break;
            }
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            RotateUnit(moveDirection);
            //CheckCurrentGridPosition(); if you want to update _currentGridPosition every time position changes uncomment this.
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
    
    public List<GridPosition> GetGridPositionsInMovementRange()
    {
        List<GridPosition> gridPositionInMovementRange = new List<GridPosition>();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)            
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = _currentGridPosition + offsetGridPosition;
                gridPositionInMovementRange.Add(testGridPosition);
            }
        }
        return gridPositionInMovementRange;
    }
}

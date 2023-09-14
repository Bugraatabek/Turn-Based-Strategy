using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event Action<bool> OnMovingStateChanged;
    public override event Action onValidGridPositionListChanged;
    public event Action onActionFinished;

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private int maxMoveDistance;
    
    private Vector3 targetPosition;
    private GridPosition _currentGridPosition;


    private void Start() 
    {
        UpdateCurrentGridPosition();
    }

    public override bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(_isActive) return false;
        if(!IsValidActionGridPosition(targetPosition)) return false;
        
        this.onActionFinished = onActionFinished; 
        _isActive = true;

        this.targetPosition = GetFinalDestination(targetPosition);
        StartCoroutine(Move());
        return true;
    }

    private IEnumerator Move()
    {
        OnMovingStateChanged?.Invoke(true);
        float stoppingDistance = 0.1f;
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= stoppingDistance) 
            {
                _isActive = false;
                OnMovingStateChanged?.Invoke(false);
                onActionFinished?.Invoke();
                UpdateCurrentGridPosition(); // updates _currentGridPosition after the action is finished.
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

    private void UpdateCurrentGridPosition()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.RemoveUnitFromGridPosition(_currentGridPosition, _unit);
        LevelGrid.Instance.SetUnitAtGridPosition(gridPosition, _unit);
        _currentGridPosition = gridPosition;
        onValidGridPositionListChanged?.Invoke();
        
    }

    private Vector3 GetFinalDestination(Vector3 targetPosition)
    {
        GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
        Vector3 finalDestination = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        return finalDestination;
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionsList = new List<GridPosition>();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)            
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = _currentGridPosition + offsetGridPosition;
                if(LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    if(testGridPosition == _currentGridPosition) continue;
                    if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                    validActionGridPositionsList.Add(testGridPosition);
                }
            }
        }
        return validActionGridPositionsList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public GridPosition GetCurrentGridPosition()
    {
        return _currentGridPosition;
    }

    public override int GetActionCost()
    {
        return 2;
    }
}

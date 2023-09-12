using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveAction))]
public class Unit : MonoBehaviour, IUnit
{
    private MoveAction _moveAction;
    private GridPosition _currentGridPosition;

    public event Action<List<GridPosition>> onValidActionGridPositionListChanged;

    private void Awake() 
    {
        _moveAction = GetComponent<MoveAction>();
    }

    private void Movement_OnGridPositionChanged(GridPosition gridPosition)
    {
        LevelGrid.Instance.RemoveUnitFromGridPosition(_currentGridPosition, this);
        _currentGridPosition = gridPosition;
        LevelGrid.Instance.SetUnitAtGridPosition(_currentGridPosition, this);
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }

    // IUnit Interface
    public void TryInvokeMovement(Vector3 targetPosition)
    {
        GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
        if(!GetValidActionGridPositionList().Contains(targetGridPosition)) return;
        
        Vector3 finalDestination = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _moveAction.TryInvokeMovement(finalDestination);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        foreach (var gridPosition in _moveAction.GetGridPositionsInMovementRange())
        {
            if(LevelGrid.Instance.IsValidGridPosition(gridPosition))
            {
                if(gridPosition == _currentGridPosition) continue; //Same Grid Position where the unit is already at.
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition)) continue; //Grid Position is already occupied with another Unit.

                validActionGridPositionList.Add(gridPosition);
            }
        }
        return validActionGridPositionList;
    }

    //Subscribing to events
    private void OnEnable() 
    {
        _moveAction.onGridPositionChanged += Movement_OnGridPositionChanged;
    }
    private void OnDisable() 
    {
        _moveAction.onGridPositionChanged -= Movement_OnGridPositionChanged;
    }
}
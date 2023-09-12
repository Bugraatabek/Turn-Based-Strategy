using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Unit : MonoBehaviour, IUnit
{
    private Movement _movement;
    private GridPosition _currentGridPosition;

    private void Awake() 
    {
        _movement = GetComponent<Movement>();
    }

    private void Movement_OnGridPositionChanged(GridPosition gridPosition)
    {
        LevelGrid.Instance.RemoveUnitFromGridPosition(_currentGridPosition, this);
        _currentGridPosition = gridPosition;
        LevelGrid.Instance.SetUnitAtGridPosition(_currentGridPosition, this);
    }

    // IUnit Interface
    public void TryInvokeMovement(Vector3 targetPosition)
    {
        _movement.TryInvokeMovement(targetPosition);
    }

    //Subscribing to events
    private void OnEnable() 
    {
        _movement.onGridPositionChanged += Movement_OnGridPositionChanged;
    }
    private void OnDisable() 
    {
        _movement.onGridPositionChanged -= Movement_OnGridPositionChanged;
    }
}
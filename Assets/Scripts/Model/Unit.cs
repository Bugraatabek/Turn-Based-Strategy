using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveAction), typeof(SpinAction))]
public class Unit : MonoBehaviour, IUnit
{
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private BaseAction selectedAction;
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged; //IUnit event
    
    
    //IUnit Interface
    public bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(selectedAction == null) return false;
        return selectedAction.TryInvokeAction(targetPosition, onActionFinished);
    }

    //IUnit Interface
    public List<GridPosition> GetValidActionGridPositionList()
    {
        return _moveAction.GetValidActionGridPositionList();
    }

    //IUnit Interface
    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    private void MoveAction_OnGridPositionChanged(GridPosition gridPosition)
    {
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }

    public GridPosition GetCurrentGridPosition()
    {
        return _moveAction.GetCurrentGridPosition();
    }
    
    private void Awake() 
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    //Subscribing to events
    private void OnEnable() 
    {
        _moveAction.onGridPositionChanged += MoveAction_OnGridPositionChanged;
    }
    private void OnDisable() 
    {
        _moveAction.onGridPositionChanged -= MoveAction_OnGridPositionChanged;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }
}
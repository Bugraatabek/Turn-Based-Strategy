using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{   
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionFinished;
    protected bool _isActive;
    protected Unit _unit;
    protected Action onActionFinished;
    public virtual event Action onValidGridPositionListChanged;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    protected virtual bool MeetsTheConditions(Vector3 targetPosition)
    {
        if(_isActive) return false;
        if(!IsValidActionGridPosition(targetPosition)) return false;
        return true;
    }

    public virtual bool IsValidActionGridPosition(Vector3 targetPosition)
    {
        GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
        
        if(!GetValidActionGridPositionList().Contains(targetGridPosition)) return false;
        return true;
    }

    protected void ActionStart(Action onActionFinished)
    {
        _isActive = true;
        this.onActionFinished = onActionFinished;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionFinished()
    {
        _isActive = false;
        onActionFinished?.Invoke();
        OnAnyActionFinished?.Invoke(this, EventArgs.Empty);
    }
    
    public abstract bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished);
    public abstract int GetActionCost();
    public abstract string GetActionName(); 
    public abstract List<GridPosition> GetValidActionGridPositionList();
    
}

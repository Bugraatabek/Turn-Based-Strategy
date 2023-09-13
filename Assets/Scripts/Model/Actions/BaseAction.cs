using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected bool _isActive;
    protected Unit _unit;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public virtual bool IsValidActionGridPosition(Vector3 targetPosition)
    {
        GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
        
        if(!GetValidActionGridPositionList().Contains(targetGridPosition)) return false;
        return true;
    }

    public abstract string GetActionName(); 
    public abstract bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished);
    public abstract List<GridPosition> GetValidActionGridPositionList();
    
}

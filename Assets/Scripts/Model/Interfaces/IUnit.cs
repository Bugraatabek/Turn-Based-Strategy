using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged;
    public bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished);
    public void SetSelectedAction(BaseAction baseAction);
    public List<GridPosition> GetValidActionGridPositionList();
    public BaseAction[] GetBaseActionArray();
}
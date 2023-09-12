using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged;
    public void TryInvokeMovement(Vector3 targetPosition);
    public List<GridPosition> GetValidActionGridPositionList();
}
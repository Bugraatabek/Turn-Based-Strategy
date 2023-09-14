using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged;
    public event Action onActionPointsChanged;
    public event Action<IUnit> onTurnFinished;

    public void SetSelectedAction(BaseAction baseAction);
    public void FinishTurn();
    public void ResetTurn();

    public bool IsEnemy();
    public bool IsTurnFinished();
    public bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished);

    public BaseAction GetSelectedAction();
    public List<GridPosition> GetValidActionGridPositionList();
    public BaseAction[] GetBaseActionArray();

    public int GetCurrentActionPoints();
    public int GetInitiative();
}
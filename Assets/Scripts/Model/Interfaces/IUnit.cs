using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged;
    public event Action onActionPointsChanged;
    public event Action<IUnit> onTurnFinished;
    public event Action<IUnit> onDead;

    public bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished);

    public void SetSelectedAction(BaseAction baseAction);
    public void FinishTurn();
    public void ResetTurn();
    public void SetInitiative(int initiative);


    public int GetInitiative();
    public bool IsEnemy();
    public bool IsTurnFinished();
    public bool IsDead();
    

    public BaseAction GetSelectedAction();
    public List<GridPosition> GetValidActionGridPositionList();
    public BaseAction[] GetBaseActionArray();

    public int GetCurrentActionPoints();
}
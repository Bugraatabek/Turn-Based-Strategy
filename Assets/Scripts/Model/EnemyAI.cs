using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextUnitInTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    IUnit _currentEnemyUnit;

    EnemyAIAction bestEnemyAIAction = null;
    

    private void Start() 
    {
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
    }

    private void ActionSystem_OnSelectedUnitChanged(IUnit unit)
    {
        if(unit == null) return;
        if(!unit.IsEnemy()) return;
        
        _currentEnemyUnit = unit;

        StartCoroutine(PlayEnemyTurn());
    }

    private IEnumerator PlayEnemyTurn()
    {
        state = State.TakingTurn;
        timer = 2f;
        
        while(true)
        {
            switch(state)
            {
                case State.WaitingForNextUnitInTurn:
                    break;
                case State.TakingTurn:
                    timer -= Time.deltaTime;
                    if(timer <= 0)
                    {
                        state = State.Busy;
                        if(!TryInvokeAIACtion(SetStateTakingTurn))
                        {
                            _currentEnemyUnit.FinishTurn();
                            bestEnemyAIAction = null;
                            yield break;
                        }
                    }
                    break;
                case State.Busy:
                    break;
            }

        yield return new WaitForEndOfFrame();
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryInvokeAIACtion(Action onEnemyAIActionComplete)
    {
        BaseAction baseAction = GetBestAction();
        ActionSystem.Instance.SetSelectedAction(baseAction);
        Vector3 targetPosition = PickPositionToUsetheAction(bestEnemyAIAction.gridPosition);

        if(!_currentEnemyUnit.TryInvokeAction(targetPosition, onEnemyAIActionComplete)) return false;
        return true;
    }

    private Vector3 PickPositionToUsetheAction(GridPosition targetGridPosition)
    {
        print("Got The Target Point");
        return LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

    private BaseAction GetBestAction()
    {
        bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in _currentEnemyUnit.GetBaseActionArray())
        {
            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }
        return bestBaseAction;
    }
}

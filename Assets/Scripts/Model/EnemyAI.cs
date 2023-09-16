using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    IUnit _currentEnemyUnit;

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
        yield return new WaitForSeconds(2);
        _currentEnemyUnit.FinishTurn();
        yield break;
    }
}

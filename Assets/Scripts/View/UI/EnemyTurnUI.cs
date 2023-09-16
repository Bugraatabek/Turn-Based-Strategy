using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnUI : MonoBehaviour
{
    void Start()
    {
        TurnSystem.Instance.onNextUnitTurn += TurnSystem_OnNextUnitTurn;
    }

    private void TurnSystem_OnNextUnitTurn(IUnit unit)
    {
        gameObject.SetActive(unit.IsEnemy());
    }
}

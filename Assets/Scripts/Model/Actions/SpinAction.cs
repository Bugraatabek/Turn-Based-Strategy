using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    public override event Action onValidGridPositionListChanged;

    public override bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(!MeetsTheConditions(targetPosition)) return false;

        //onValidGridPositionListChanged?.Invoke();
        
        ActionStart(onActionFinished);
        StartCoroutine(Spin());
        return true;
    }

    private IEnumerator Spin()
    {
        while(true)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3 (0,spinAddAmount,0);

            _totalSpinAmount += spinAddAmount;
            if(_totalSpinAmount >= 360f) 
            {
                _totalSpinAmount = 0;
                ActionFinished();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        return new List<GridPosition>
        {
            currentGridPosition
        };
    }

    public override int GetActionCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    public event Action onActionFinished;

    public override bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(_isActive) return false;
        if(!IsValidActionGridPosition(targetPosition)) return false;
        
        this.onActionFinished = onActionFinished;
        _isActive = true;
        
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
                _isActive = false;
                onActionFinished?.Invoke();
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
        GridPosition currentGridPosition = _unit.GetCurrentGridPosition();

        return new List<GridPosition>
        {
            currentGridPosition
        };
    }
}

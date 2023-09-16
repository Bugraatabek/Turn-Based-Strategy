using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    public event Action<Vector3> onShoot;
    public override event Action onValidGridPositionListChanged;

    [SerializeField] private int _maxRange;
    private State _state;
    private float _stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;


    public override bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(!MeetsTheConditions(targetPosition)) return false;

        
        canShootBullet = true;

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetPosition);
        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;
        
        ActionStart(onActionFinished);
        StartCoroutine(ShootingRoutine());
        return true;
    }

    private IEnumerator ShootingRoutine()
    {
        while(true)
        {
            _stateTimer -= Time.deltaTime;
            
            if(_stateTimer <= 0)
            {
                NextState();
            }

            switch(_state)
            {
                case State.Aiming:
                    Aim();
                    break;
                case State.Shooting:
                    if(canShootBullet)  { Shoot(); }
                    break;
                case State.Cooloff:
                    ActionFinished();
                    yield break;
            }

            yield return new WaitForEndOfFrame();
        }
        
    }

    private void NextState()
    {
        switch(_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = 0.1f;
                _stateTimer = shootingStateTime;
            break;
            case State.Shooting:
                _state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                _stateTimer = coolOffStateTime;
            break;
            case State.Cooloff:
            break;
        } 
    }

    private void Aim()
    {
        Vector3 aimDir = (targetUnit.transform.position - transform.position).normalized;
        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotationSpeed);
    }

    private void Shoot()
    {
        targetUnit.TakeDamage(40);
        if(targetUnit.IsDead()) { onValidGridPositionListChanged?.Invoke(); }
        onShoot?.Invoke(targetUnit.transform.position);
        canShootBullet = false;
    }

    public override int GetActionCost()
    {
        return 3;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionsList = new List<GridPosition>();
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        for (int x = -_maxRange; x <= _maxRange; x++)
        {
            for (int z = -_maxRange; z <= _maxRange; z++)            
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = currentGridPosition + offsetGridPosition;
                if(LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    int textDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if(textDistance > _maxRange) continue;
                    if(testGridPosition == currentGridPosition) continue;
                    if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if(targetUnit.IsEnemy() == _unit.IsEnemy()) continue;
                    
                    validActionGridPositionsList.Add(testGridPosition);
                }
            }
        }
        return validActionGridPositionsList;
    }


    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public Unit GetUnit()
    {
        return _unit;
    }

    public int GetRange()
    {
        return _maxRange;
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Count;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }
}
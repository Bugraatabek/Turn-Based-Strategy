using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour, IUnit
{
    public event Action<List<GridPosition>> onValidActionGridPositionListChanged; //IUnit event
    public event Action onActionPointsChanged;
    public event Action<IUnit> onTurnFinished;
    public event Action<IUnit> onDead;
    [SerializeField] private bool isEnemy;

    private Health _health;
    private BaseAction[] _baseActionArray;
    private BaseAction _selectedAction;
    
    [SerializeField] private int _initiative;
    private bool _isTurnFinished = false;
    private int _maxActionPoints = 10;
    private int _currentActionPoints;
    

    private void Awake() 
    {
        _baseActionArray = GetComponents<BaseAction>();
        _health = GetComponent<Health>();
        _currentActionPoints = _maxActionPoints;
    }

    private void Start() 
    {
        _health.onDead += Health_OnDead;
    }

    private void Health_OnDead()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.RemoveUnitFromGridPosition(gridPosition, this);
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
        onDead?.Invoke(this);
        Destroy(gameObject);
    }

    //IUnit Interface
    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }

    //IUnit Interface
    public bool TryInvokeAction(Vector3 targetPosition, Action onActionFinished)
    {
        if(_selectedAction == null) return false;
        if(!HasEnoughActionPoints(_selectedAction)) return false;
        if(_selectedAction.TryInvokeAction(targetPosition, onActionFinished))
        {
            SpendActionPoints(_selectedAction.GetActionCost());
            return true;
        }
        return false;
    }

    //IUnit Interface
    public List<GridPosition> GetValidActionGridPositionList()
    {
        if(_selectedAction == null) return null;
        if(_isTurnFinished == true) return null; 
        return _selectedAction.GetValidActionGridPositionList();
    }

    //IUnit Interface
    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    //IUnit Interface
    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }

    //IUnit Interface
    public int GetCurrentActionPoints()
    {
        return _currentActionPoints;
    }

    //IUnit Interface
    public void FinishTurn()
    {
        _isTurnFinished = true;
        onTurnFinished?.Invoke(this);
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }

    //IUnit Interface
    public bool IsTurnFinished()
    {
        return _isTurnFinished;
    }

    //IUnit Interface
    public void ResetTurn()
    {
        _isTurnFinished = false;
        ResetActionPoints();
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }

    private bool HasEnoughActionPoints(BaseAction baseAction)
    {
        if(baseAction.GetActionCost() <= _currentActionPoints) return true;
        return false;
    }

    private void ResetActionPoints()
    {
        _currentActionPoints = _maxActionPoints;
        onActionPointsChanged?.Invoke();
    }

    private void SpendActionPoints(int amount)
    {
        _currentActionPoints -= amount;
        onActionPointsChanged?.Invoke();
    }

    private void BaseAction_OnValidGridPositionListChanged()
    {
        onValidActionGridPositionListChanged?.Invoke(GetValidActionGridPositionList());
    }
    
    //Subscribing to events
    private void OnEnable() 
    {
        foreach (var baseAction in _baseActionArray)
        {
            baseAction.onValidGridPositionListChanged += BaseAction_OnValidGridPositionListChanged;
        }
    }

    private void OnDisable() 
    {
        foreach (var baseAction in _baseActionArray)
        {
            baseAction.onValidGridPositionListChanged -= BaseAction_OnValidGridPositionListChanged;
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public bool IsDead()
    {
        return _health.IsDead();
    }

    public void SetInitiative(int initiative)
    {
        _initiative = initiative;
    }

    public int GetInitiative()
    {
        return _initiative;
    }

    public void TakeDamage(int damageAmount)
    {
        _health.TakeDamage(damageAmount);
    }

    //IUnit interface
    public GridPosition GetGridPosition()
    {
        return LevelGrid.Instance.GetGridPosition(transform.position);
    }
}
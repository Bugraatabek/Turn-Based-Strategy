using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    public static ActionSystem Instance {get; private set;}
    
    public IUnit SelectedUnit {get {return selectedUnit;}}

    public event Action<IUnit> onSelectedUnitChanged;
    public event Action onSelectedActionChanged;
    public event Action<List<GridPosition>> onSelectedUnitChangedValidActionList;
    public event Action onSelectedUnitSpendActionPoints;
    public event Action<bool> onBusyChanged;


    private IUnit selectedUnit;
    private bool _isBusy;
    
    
    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        if(_isBusy) return;
        //if(selectedUnit.IsTurnFinished()) return; If you want to lock actions after turn is finished uncomment.
        selectedUnit.SetSelectedAction(baseAction);
        onSelectedActionChanged?.Invoke();
    }

    private void RaycastController_onClickGrid(Vector3 targetPosition)
    {
        if(_isBusy) return;
        if(selectedUnit == null) return;
        if(selectedUnit.IsTurnFinished()) return;
        if(selectedUnit.TryInvokeAction(targetPosition,ClearIsBusy) == true) SetIsBusy();
        
    }

    private void RaycastController_onClickUnit(IUnit unit, Vector3 targetPosition)
    {
        //if(unit.IsEnemy()) return;
        if(_isBusy) return;
        if(!unit.IsEnemy()) return;
        if(selectedUnit.TryInvokeAction(targetPosition,ClearIsBusy) == true) SetIsBusy();
        
        //ChangeSelectedUnit(unit);
    }

    private void IUnit_OnValidActionGridPositionListChanged(List<GridPosition> newValidGridPositionsList)
    {
        onSelectedUnitChangedValidActionList?.Invoke(newValidGridPositionsList);
    }

    private void IUnit_ActionPointsChanged()
    {
        onSelectedUnitSpendActionPoints?.Invoke();
    }

    private void TurnSystem_OnNextUnitTurn(IUnit unit)
    {
        ChangeSelectedUnit(unit);
    }

    private void SetIsBusy()
    {
        _isBusy = true;
        onBusyChanged?.Invoke(true);
    }

    private void ClearIsBusy()
    {
        _isBusy = false;
        onBusyChanged?.Invoke(false);
    }

    private void ChangeSelectedUnit(IUnit unit)
    {
        if(_isBusy) return;
        // if(unit.IsEnemy())
        if(selectedUnit != null) 
        {
            selectedUnit.onValidActionGridPositionListChanged -= IUnit_OnValidActionGridPositionListChanged;
            selectedUnit.onActionPointsChanged -= IUnit_ActionPointsChanged;
        }
        unit.onValidActionGridPositionListChanged += IUnit_OnValidActionGridPositionListChanged;
        unit.onActionPointsChanged += IUnit_ActionPointsChanged;
        
        selectedUnit = unit;
        onSelectedUnitChanged?.Invoke(unit);
    }

    private void Start() 
    {
        RaycastController.Instance.onClickedGrid += RaycastController_onClickGrid;
        RaycastController.Instance.onClickedUnit += RaycastController_onClickUnit;
        TurnSystem.Instance.onNextUnitTurn += TurnSystem_OnNextUnitTurn;
    }

   
    private void OnDisable() 
    {
        RaycastController.Instance.onClickedGrid -= RaycastController_onClickGrid;
        RaycastController.Instance.onClickedUnit -= RaycastController_onClickUnit;
        TurnSystem.Instance.onNextUnitTurn -= TurnSystem_OnNextUnitTurn;
    }
}

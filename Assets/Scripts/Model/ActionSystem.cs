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
    public event Action<List<GridPosition>> onSelectedUnitChangedPosition;


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
        selectedUnit.SetSelectedAction(baseAction);
    }

    private void RaycastController_onClickGrid(Vector3 targetPosition)
    {
        if(_isBusy) return;
        if(selectedUnit?.TryInvokeAction(targetPosition,ClearIsBusy) == true) _isBusy = true;
    }

    private void RaycastController_onClickUnit(IUnit unit)
    {
        if(_isBusy) return;
        ChangeSelectedUnit(unit);
    }

    private void IUnit_OnGridPositionChanged(List<GridPosition> newValidGridPositionsList)
    {
        onSelectedUnitChangedPosition?.Invoke(newValidGridPositionsList);
    }

    private void ClearIsBusy()
    {
        _isBusy = false;
    }

    private void ChangeSelectedUnit(IUnit unit)
    {
        if(selectedUnit != null) 
        {
            selectedUnit.onValidActionGridPositionListChanged -= IUnit_OnGridPositionChanged;
        }
        unit.onValidActionGridPositionListChanged += IUnit_OnGridPositionChanged;
        
        selectedUnit = unit;
        onSelectedUnitChanged?.Invoke(unit);
    }

    private void Start() 
    {
        RaycastController.Instance.onClickedGrid += RaycastController_onClickGrid;
        RaycastController.Instance.onClickedUnit += RaycastController_onClickUnit;
    }

    private void OnDisable() 
    {
        RaycastController.Instance.onClickedGrid -= RaycastController_onClickGrid;
        RaycastController.Instance.onClickedUnit -= RaycastController_onClickUnit;
    }
}

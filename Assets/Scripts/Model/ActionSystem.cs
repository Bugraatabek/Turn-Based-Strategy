using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    public static ActionSystem Instance {get; private set;}
    public event Action<IUnit> onSelectedUnitChanged;

    public event Action<List<GridPosition>> onSelectedUnitChangedPosition;

    private static IUnit selectedUnit;
    
    
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
    
    private void RaycastController_onClickGrid(Vector3 targetPosition)
    {
        MoveSelectedUnit(targetPosition);
    }

    private void RaycastController_onClickUnit(IUnit unit)
    {
        ChangeSelectedUnit(unit);
    }

    private void MoveSelectedUnit(Vector3 targetPosition)
    {
        selectedUnit?.TryInvokeMovement(targetPosition);
    }

    private void ChangeSelectedUnit(IUnit unit)
    {
        if(selectedUnit != null) selectedUnit.onValidActionGridPositionListChanged -= IUnit_OnGridPositionChanged;
        unit.onValidActionGridPositionListChanged += IUnit_OnGridPositionChanged;
        selectedUnit = unit;
        
        onSelectedUnitChanged?.Invoke(unit);
    }

    private void IUnit_OnGridPositionChanged(List<GridPosition> newValidGridPositionsList)
    {
        onSelectedUnitChangedPosition?.Invoke(newValidGridPositionsList);
    }

    private void OnEnable() 
    {
        RaycastController.onClickedGrid += RaycastController_onClickGrid;
        RaycastController.onClickedUnit += RaycastController_onClickUnit;
    }

    private void OnDisable() 
    {
        RaycastController.onClickedGrid -= RaycastController_onClickGrid;
        RaycastController.onClickedUnit -= RaycastController_onClickUnit;
    }
}

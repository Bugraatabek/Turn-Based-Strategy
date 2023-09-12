using System;
using System.Collections;
using System.Collections.Generic;
using Control;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    public static event Action<IUnit> onSelectedUnitChanged;

    private static IUnit selectedUnit;
    public static ActionSystem Instance {get; private set;}
    
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
        selectedUnit = unit;
        onSelectedUnitChanged?.Invoke(unit);
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

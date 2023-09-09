using System.Collections;
using System.Collections.Generic;
using Control;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    private IUnit selectedUnit;
    private static ActionSystem Instance;
    
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

    private void OnEnable() 
    {
        RaycastController.actionSystemMoveSelectedUnit += MoveSelectedUnit;
        RaycastController.actionSystemChangeSelectedUnit += ChangeSelectedUnit;
    }

    private void OnDisable() 
    {
        RaycastController.actionSystemMoveSelectedUnit -= MoveSelectedUnit;
        RaycastController.actionSystemChangeSelectedUnit -= ChangeSelectedUnit;
    }
    
    private void MoveSelectedUnit(Vector3 targetPosition)
    {
        if(selectedUnit == null) return;
        selectedUnit.TryInvokeMovement(targetPosition);
    }

    private void ChangeSelectedUnit(IUnit unit)
    {
        print("ActionSystem, ChangeSelectedUnit");
        selectedUnit = unit;
    }
}

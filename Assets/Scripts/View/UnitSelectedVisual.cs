using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] Unit parentUnit;
    private IUnit iUnit;

    private void Awake() 
    {
        iUnit = GetComponent<IUnit>();
    }

    private void Start() 
    {
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChange;
    }

    private void OnDisable() 
    {
        ActionSystem.Instance.onSelectedUnitChanged -= ActionSystem_OnSelectedUnitChange;
    }

    private void ActionSystem_OnSelectedUnitChange(IUnit unit)
    {
        if(iUnit == unit)
        {
            _meshRenderer.enabled = true;
            return;
        }
        _meshRenderer.enabled = false;
    }
}

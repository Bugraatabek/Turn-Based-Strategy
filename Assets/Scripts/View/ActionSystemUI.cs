using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;

    private IUnit _selectedUnit = null;

    private void Start()
    {
        ActionSystem.Instance.onSelectedUnitChanged += UpdateUnitActionButtons;
        ClearUnitActionButtons();
    }

    private void UpdateUnitActionButtons(IUnit selectedUnit)
    {
        if(_selectedUnit == selectedUnit) return;
        _selectedUnit = selectedUnit;

        ClearUnitActionButtons();
        
        if(_selectedUnit == null) return;
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUIInstance = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUIInstance.SetBaseAction(baseAction);
        }
    }

    private void ClearUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }
    }
}

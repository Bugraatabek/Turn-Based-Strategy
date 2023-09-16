using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private IUnit _selectedUnit = null;
    private List<ActionButtonUI> _actionButtonInstanceList = new List<ActionButtonUI>();

    private void Start()
    {
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
        ActionSystem.Instance.onSelectedActionChanged += ActionSystem_OnSelectedActionChanged;
        ActionSystem.Instance.onSelectedUnitSpendActionPoints += ActionSystem_OnSelectedUnitSpendActionPoints;
        ClearUnitActionButtons();
        UpdateActionPointsText();
    }

    private void ActionSystem_OnSelectedUnitSpendActionPoints()
    {
        UpdateActionPointsText();
    }

    private void ActionSystem_OnSelectedUnitChanged(IUnit selectedUnit)
    {
        UpdateUnitActionButtons(selectedUnit);
        UpdateSelectedActionVisual(selectedUnit.GetSelectedAction());
        UpdateActionPointsText();
    }

    private void ActionSystem_OnSelectedActionChanged(BaseAction selectedAction)
    {
        UpdateSelectedActionVisual(selectedAction);
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
            _actionButtonInstanceList.Add(actionButtonUIInstance);
        }
    }

    private void ClearUnitActionButtons()
    {
        _actionButtonInstanceList.Clear();
        foreach (Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }
    }

    private void UpdateSelectedActionVisual(BaseAction selectedAction)
    {
        foreach (ActionButtonUI actionButtonInstance in _actionButtonInstanceList)
        {
            actionButtonInstance.SetSelectedVisual(selectedAction);
        }
    }

    private void UpdateActionPointsText()
    {
        if(_selectedUnit == null)
        {
            actionPointsText.gameObject.SetActive(false);
            return;
        }
        actionPointsText.gameObject.SetActive(true);
        actionPointsText.text = $"Action Points : {_selectedUnit.GetCurrentActionPoints()}";
    }

    private void OnDisable()
    {
        ActionSystem.Instance.onSelectedUnitChanged -= ActionSystem_OnSelectedUnitChanged;
        ActionSystem.Instance.onSelectedActionChanged -= ActionSystem_OnSelectedActionChanged;
        ActionSystem.Instance.onSelectedUnitSpendActionPoints -= ActionSystem_OnSelectedUnitSpendActionPoints;
    }
}

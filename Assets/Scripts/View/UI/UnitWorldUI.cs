using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Health _unitHealth;


    private void Start() 
    {
        unit.onActionPointsChanged += Unit_OnActionPointsChanged;
        _unitHealth.onTakeDamage += Unit_OnTakeDamage;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void Unit_OnTakeDamage()
    {
        UpdateHealthBar();
    }

    private void Unit_OnActionPointsChanged()
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetCurrentActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = _unitHealth.GetHealthNormalized();
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour 
{
    [SerializeField] Button _nextTurnButton;
    [SerializeField] Button _endTurnButton;
    [SerializeField] TextMeshProUGUI _turnNumberText;
    [SerializeField] GameObject _turnFinishedTextGameObject;

    private IUnit _selectedUnit;

    private void Start() 
    {
        //TurnSystem.Instance.onEveryTurnFinished += TurnSystem_OnEveryTurnFinished;
        TurnSystem.Instance.onNewTurn += TurnSystem_OnNewTurn;
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
        
        // _nextTurnButton.onClick.AddListener(() => 
        // {
        //     TurnSystem.Instance.NextTurn();
        // });

        _selectedUnit = ActionSystem.Instance.SelectedUnit;
        StartCoroutine(WaitAWhileBeforeUpdatingUI()); // Because There is a Race Condition With Turn System
    }

    private void TurnSystem_OnNewTurn()
    {
        UpdateUI();
    }

    // private void TurnSystem_OnEveryTurnFinished()
    // {
    //     UpdateUI();
    // }

    private void ActionSystem_OnSelectedUnitChanged(IUnit unit)
    {
        if(_selectedUnit != null)
        {
            _endTurnButton.onClick.RemoveListener(_selectedUnit.FinishTurn);
        }
        _endTurnButton.onClick.AddListener(unit.FinishTurn);
        _selectedUnit = unit;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        _turnNumberText.text = $"Turn {TurnSystem.Instance.CurrentTurn}";
        
        _nextTurnButton.gameObject.SetActive(false);
        _endTurnButton.gameObject.SetActive(true);
        _turnFinishedTextGameObject.SetActive(false);

        if(_selectedUnit.IsEnemy()) 
        {
            _endTurnButton.gameObject.SetActive(false);
            return;
        }

        // if(TurnSystem.Instance.IsEveryTurnFinished())
        // {
        //     _nextTurnButton.gameObject.SetActive(true);
        //     _endTurnButton.gameObject.SetActive(false);
        //     return;
        // }

        // if(_selectedUnit == null)
        // {
        //     _nextTurnButton.gameObject.SetActive(false);
        //     _endTurnButton.gameObject.SetActive(false);
        //     return;
        // }

        // if(_selectedUnit != null && _selectedUnit.IsTurnFinished())
        // {
        //     _nextTurnButton.gameObject.SetActive(false);
        //     _endTurnButton.gameObject.SetActive(false);
        //     _turnFinishedTextGameObject.gameObject.SetActive(true);
        //     return;
        // }
    }

    private IEnumerator WaitAWhileBeforeUpdatingUI()
    {
        yield return new WaitForSeconds(.1f);
        UpdateUI();
        yield break;
    }
}
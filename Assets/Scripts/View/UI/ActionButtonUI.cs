using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button button;
    [SerializeField] private Image selectedVisual;
    private BaseAction _baseAction;
    
    public void SetBaseAction(BaseAction baseAction)
    {
        _buttonText.text = baseAction.GetActionName();
        _baseAction = baseAction;
        button.onClick.AddListener(() => {
            ActionSystem.Instance.SetSelectedAction(baseAction);
        });

    }

    public void SetSelectedVisual(BaseAction baseAction)
    {
        if(baseAction == _baseAction)
        {
            selectedVisual.enabled = true;
            return;
        }
        selectedVisual.enabled = false;
    }
}

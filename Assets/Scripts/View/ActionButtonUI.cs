using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button button;

    public void SetBaseAction(BaseAction baseAction)
    {
        _buttonText.text = baseAction.GetActionName();
        button.onClick.AddListener(() => {
            ActionSystem.Instance.SetSelectedAction(baseAction);
        });

    }
}

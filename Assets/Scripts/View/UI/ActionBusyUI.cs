using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start() 
    {
        ActionSystem.Instance.onBusyChanged += ActionSystem_OnBusyChanged;
        ShowHideUI(false);
    }

    private void ActionSystem_OnBusyChanged(bool shouldActivate)
    {
        ShowHideUI(shouldActivate);
    }

    public void ShowHideUI(bool shouldActivate)
    {
        gameObject.SetActive(shouldActivate);
    }
}

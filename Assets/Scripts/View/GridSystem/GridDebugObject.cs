using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private object _gridObject;
    [SerializeField] private TextMeshPro _gridText;


    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }

    protected virtual void Update() 
    {
        _gridText.text = _gridObject.ToString();
    }
}

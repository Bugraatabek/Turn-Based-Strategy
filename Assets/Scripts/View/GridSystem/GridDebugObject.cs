using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject _gridObject;
    [SerializeField] private TextMeshPro _gridText;

    
    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }

    private void Update() 
    {
        _gridText.text = _gridObject.ToString();
    }
}

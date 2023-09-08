using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public static event Action<Vector3> movementTryInvokeMovement;

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            movementTryInvokeMovement?.Invoke(MouseWorld.GetMousePosition());
        }
    }
}

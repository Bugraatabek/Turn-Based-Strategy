using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class InputReader : MonoBehaviour
    {
        private void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastController.Raycast();
            }
        }
    }
}

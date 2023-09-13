using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class InputReader : MonoBehaviour
    {
        public static InputReader Instance {get; private set;}
        public event Action<Vector3> onWASDPressed;
        public event Action<Vector3> onQEPressed;
        public event Action<int> onMouseButtonDown;
        
        private void Awake() 
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        private void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                onMouseButtonDown?.Invoke(0);
            }

            // if(Input.GetMouseButtonDown(1))
            // {
            //     onMouseButtonDown?.Invoke(1);
            // }

            if(Input.GetKey(KeyCode.W))
            {
                onWASDPressed?.Invoke(new Vector3(0,0,+1));
            }
            if(Input.GetKey(KeyCode.S))
            {
                onWASDPressed?.Invoke(new Vector3(0,0,-1));
            }
            if(Input.GetKey(KeyCode.D))
            {
                onWASDPressed?.Invoke(new Vector3(+1,0,0));
            }
            if(Input.GetKey(KeyCode.A))
            {
                onWASDPressed?.Invoke(new Vector3(-1,0,0));
            }


            if(Input.GetKey(KeyCode.Q))
            {
                onQEPressed?.Invoke(new Vector3(0,+1f,0));
            }
            if(Input.GetKey(KeyCode.E))
            {
                onQEPressed?.Invoke(new Vector3(0,-1f,0));
            }
        }
    }
}

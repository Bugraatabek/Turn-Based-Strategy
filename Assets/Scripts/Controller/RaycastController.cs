using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class RaycastController : MonoBehaviour
    {
        public static event Action<Vector3> onClickedGrid;
        public static event Action<IUnit> onClickedUnit;


        private static RaycastController instance;
        [SerializeField] private LayerMask _layerMask;
        
        private Vector3 _mousePosition;

        private void Awake() 
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public static void Raycast()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance._layerMask);
            IUnit unit;
            if(instance.TryGetAUnit(raycastHit, out unit))
            {
                onClickedUnit?.Invoke(unit);
                print("RaycastController : A Unit Found, Calling On Click Unit Event");
            }
            else
            {
                onClickedGrid?.Invoke(raycastHit.point);

                print("RaycastController : Calling On Click Grid Event");
            }
        }

        private bool TryGetAUnit(RaycastHit raycastHit, out IUnit unit)
        {
            return raycastHit.transform.gameObject.TryGetComponent<IUnit>(out unit);
        }

        public static Vector3 GetMousePosition()
        {
            return instance._mousePosition;
        }

        

        

        
    }
}

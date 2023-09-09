using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class RaycastController : MonoBehaviour
    {
    
        public static event Action<Vector3> actionSystemMoveSelectedUnit;
        public static event Action<IUnit> actionSystemChangeSelectedUnit;


        private static RaycastController instance;
        [SerializeField] private LayerMask _layerMask;
        
        private Vector3 _mousePosition;
        private GameObject _lastHitGameObject;
        private IUnit _lastFoundUnit;

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

            instance._lastHitGameObject = raycastHit.transform.gameObject;
            instance._mousePosition = raycastHit.point;

            instance.CallRightEvent();
        }

        private Vector3 GetMousePosition()
        {
            return _mousePosition;
        }

        private IUnit GetLastFoundUnit()
        {
            return _lastFoundUnit;
        }

        private bool HasFoundAUnit()
        {
            if(_lastHitGameObject.TryGetComponent<IUnit>(out IUnit unit)) 
            {
                _lastFoundUnit = unit;
                return true;
            }
            return false;
        }

        private void CallRightEvent()
        {
            if(HasFoundAUnit()) 
            {
                actionSystemChangeSelectedUnit?.Invoke(GetLastFoundUnit());
                print("RaycastController : A Unit Found, Calling Change Selected Unit Event");
                return;
            }

            actionSystemMoveSelectedUnit?.Invoke(GetMousePosition());
            print("RaycastController : Calling Move Selected Unit Event");
        }
    }
}

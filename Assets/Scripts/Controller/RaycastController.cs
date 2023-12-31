using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class RaycastController : MonoBehaviour
    {
        public static RaycastController Instance {get; private set;}
        public event Action<Vector3> onClickedGrid;
        public event Action<IUnit, Vector3> onClickedUnit;

        [SerializeField] private LayerMask _layerMask;
        
        private Vector3 _mousePosition;

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

        private void InputReader_OnMouseButtonDown0(int pressedButtonNumber)
        {
            Raycast(pressedButtonNumber);
        }

        public void Raycast(int pressedButtonNumber)
        {
            if(pressedButtonNumber != 0) return;
            if(EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance._layerMask))
            {
                if(TryGetAUnit(raycastHit, out IUnit unit))
                {
                    onClickedUnit?.Invoke(unit, raycastHit.point);
                    print("RaycastController : A Unit Found, Calling On Click Unit Event");
                }
                else
                {
                    onClickedGrid?.Invoke(raycastHit.point);
                    print("RaycastController : Calling On Click Grid Event");
                }
            } 
        }

        private bool TryGetAUnit(RaycastHit raycastHit, out IUnit unit)
        {
            return raycastHit.transform.gameObject.TryGetComponent<IUnit>(out unit);
        }

        private bool TryGetAGrid(RaycastHit ray)
        {

            return false;
        }

        public Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance._layerMask);
            return raycastHit.point;

        }

        private void Start() 
        {
            InputReader.Instance.onMouseButtonDown += InputReader_OnMouseButtonDown0;
        }

        private void OnDisable() 
        {
            InputReader.Instance.onMouseButtonDown -= InputReader_OnMouseButtonDown0;
        }
    }
}

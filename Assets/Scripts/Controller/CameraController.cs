using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float _MOVE_SPEED = 10f;
    private const float _ROTATION_SPEED = 100f;
    private const float _MIN_FOLLOW_Y_OFFSET = 2f;
    private const float _MAX_FOLLOW_Y_OFFSET = 12f;
    private const float _ZOOM_SPEED = 5f;
    private const float _ZOOM_AMOUNT = 1f;

    private Vector3 _targetFollowOffset;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    CinemachineTransposer _cinemachineTransposer;
    
    private void Start() 
    {
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector3 inputMoveDirection = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = +1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }
        transform.Translate(inputMoveDirection * Time.deltaTime * _MOVE_SPEED);
    }

    private void RotateCamera()
    {
        Vector3 rotationVector = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        transform.eulerAngles += rotationVector * _ROTATION_SPEED * Time.deltaTime;
    }

    private void ZoomCamera()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= _ZOOM_AMOUNT;
        }
        
        if(Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += _ZOOM_AMOUNT;
        }
       
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, _MIN_FOLLOW_Y_OFFSET, _MAX_FOLLOW_Y_OFFSET);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * _ZOOM_SPEED);
    }

    
}

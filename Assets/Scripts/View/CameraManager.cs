using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start() 
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionFinished += BaseAction_OnAnyActionFinished;
        HideActionCamera();
    }

    private void BaseAction_OnAnyActionFinished(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Unit targetUnit = shootAction.GetTargetUnit();
                Unit shooterUnit = shootAction.GetUnit();

                Vector3 shootDir = (targetUnit.transform.position - shooterUnit.transform.position).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.transform.position + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.transform.position + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}

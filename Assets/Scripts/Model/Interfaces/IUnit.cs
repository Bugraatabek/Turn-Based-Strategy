using System.Collections;
using UnityEngine;

public interface IUnit
{
    public void TryInvokeMovement(Vector3 targetPosition);
}
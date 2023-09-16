using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UnitAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;
   

    private void Awake() 
    {
        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += ShootAction_OnShoot;
        }
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMovingStateChanged += MoveAction_IsMoving;
        }
    }

    private void ShootAction_OnShoot(Vector3 targetPosition)
    {
        Shoot(targetPosition);
    }

    private void MoveAction_IsMoving(bool isMoving)
    {
        Walk(isMoving);
    }

    private void Walk(bool isMoving)
    {
        _animator.SetBool("IsMoving", isMoving);
    }

    private void Shoot(Vector3 targetUnitPosition)
    {
        _animator.SetTrigger("Shoot");
        Transform bulletProjectileTransformInstance = Instantiate(_bulletProjectilePrefab,_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectileInstance = bulletProjectileTransformInstance.GetComponent<BulletProjectile>();
        
        targetUnitPosition.y = _shootPointTransform.position.y;
        bulletProjectileInstance.Setup(targetUnitPosition);
    } 
}

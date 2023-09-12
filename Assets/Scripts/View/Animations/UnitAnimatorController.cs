using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UnitAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private MoveAction _movement;

    private void Awake() 
    {
        _movement = GetComponent<MoveAction>();
    }

    private void OnEnable() 
    {
        _movement.OnMovingStateChanged += Movement_IsMoving;
    }

    private void OnDisable() 
    {
        _movement.OnMovingStateChanged -= Movement_IsMoving;
    }

    private void Movement_IsMoving(bool isMoving)
    {
        Walk(isMoving);
    }

    public void Walk(bool isMoving)
    {
        _animator.SetBool("IsMoving", isMoving);
    }
}

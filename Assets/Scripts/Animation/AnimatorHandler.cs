using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Movement _movement;

    private void Awake() 
    {
        _movement = GetComponent<Movement>();
    }

    private void OnEnable() 
    {
        _movement.animatorHandlerWalk += Walk;
    }

    private void OnDisable() 
    {
        _movement.animatorHandlerWalk -= Walk;
    }

    private void Walk(bool IsWalking)
    {
        animator.SetBool("IsMoving", IsWalking);
    }
}

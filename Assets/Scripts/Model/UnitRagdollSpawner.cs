using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private Health _health;

    private void Awake() {
        _health = GetComponent<Health>();
    }

    private void OnEnable() {
        _health.onDead += Health_OnDead;
    }

    private void Health_OnDead()
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab,transform.position,transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}

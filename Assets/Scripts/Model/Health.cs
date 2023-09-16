using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action onDead;
    public event Action onTakeDamage;

    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private bool _isDead;

    private void Awake() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        onTakeDamage?.Invoke();
        if(_currentHealth < 0) { _currentHealth = 0; }
        if(_currentHealth == 0) { Die(); }
    }

    private void Die()
    {
        _isDead = true;
        onDead?.Invoke();
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public float GetHealthNormalized()
    {
        return (float)_currentHealth / _maxHealth;
    }
}

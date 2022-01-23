using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] HealthViewer healthViewer;
    [SerializeField] int maxHealth;
    int currentHealth;

    public Action OnCharacterDead;
    public Action<int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged += healthViewer.UpdateHealthCount;
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0) OnCharacterDead?.Invoke();
            
        }
    }

    private void OnDestroy()
    {
        OnHealthChanged -= healthViewer.UpdateHealthCount;
    }
}

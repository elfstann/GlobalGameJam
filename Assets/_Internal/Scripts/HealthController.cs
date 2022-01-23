using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] HealthViewer healthViewer;
    [SerializeField] int maxHealth;

    //Move this shit somewhere else because it doesn't belong here
    [Header("Visual Settings")]
    [SerializeField] GameObject vfxOnRemove;
    [SerializeField] GameObject vfxOnAdd;

    int currentHealth;
    int prevHealth;

    public Action OnCharacterDead;
    public Action<int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged += healthViewer.UpdateHealthCount;
    }
    private void RepresentHealth()
    {
        GameObject vfxToSpawn = currentHealth > prevHealth ? vfxOnAdd : vfxOnRemove;
        if (vfxToSpawn != null) Instantiate(vfxToSpawn, PlayerController.Instance.player.transform);
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            prevHealth = currentHealth;
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            RepresentHealth();
            OnHealthChanged?.Invoke(currentHealth);
            if (currentHealth <= 0) OnCharacterDead?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnHealthChanged -= healthViewer.UpdateHealthCount;
    }
}

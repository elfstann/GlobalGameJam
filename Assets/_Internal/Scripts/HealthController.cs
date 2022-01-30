using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] HealthViewer healthViewer;
    [SerializeField] int maxHealth;

    //Move this shit somewhere else because it doesn't belong here
    [Header("Visual Settings")]
    [SerializeField] GameObject vfxOnRemove;
    [SerializeField] GameObject vfxOnAdd;
    [SerializeField] Sprite healthImage;

    int currentHealth;
    int prevHealth;

    public Action OnCharacterDead;
    public Action<int, Sprite> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged += healthViewer.UpdateHealthCount;
        OnCharacterDead += PauseManager.Instance.EndGame;
    }
    private void RepresentHealth()
    {
        GameObject vfxToSpawn = currentHealth > prevHealth ? vfxOnAdd : vfxOnRemove;
        if (vfxToSpawn != null) Instantiate(vfxToSpawn, PlayerController.Instance.player.transform);
    }
    public Sprite HealthImage { get { return healthImage; } }
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            prevHealth = currentHealth;
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            RepresentHealth();
            OnHealthChanged?.Invoke(currentHealth, healthImage);
            if (currentHealth <= 0) OnCharacterDead?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnHealthChanged -= healthViewer.UpdateHealthCount;
    }
}

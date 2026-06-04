using UnityEngine;
using System;

/// <summary>
/// Tracks all real-time character statistics during combat.
/// Includes health, stamina, rage, and combat metrics.
/// </summary>
[System.Serializable]
public class CharacterStats
{
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float currentHP;
    
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    
    [SerializeField] private float maxRage = 100f;
    [SerializeField] private float currentRage;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float power = 1f;      // Damage multiplier
    [SerializeField] private float defense = 10f;   // Defense percentage
    [SerializeField] private float stamina = 1f;    // Stamina multiplier
    
    [SerializeField] private float staminaRegenRate = 15f; // Per second
    [SerializeField] private float rageGainPerHit = 5f;
    [SerializeField] private float rageGainPerDamage = 0.1f;
    
    private int comboCount;
    private float lastDamageTime;
    
    // Events
    public static event Action<float> OnHealthChanged;
    public static event Action<float> OnStaminaChanged;
    public static event Action<float> OnRageChanged;
    public static event Action OnComboCountChanged;
    
    public CharacterStats()
    {
        ResetToFull();
    }
    
    public void ResetToFull()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        currentRage = 0f;
        comboCount = 0;
    }
    
    public void Update(float deltaTime)
    {
        // Regenerate stamina
        RegenerateStamina(deltaTime);
        
        // Natural rage decay (slow)
        DecayRage(deltaTime);
    }
    
    private void RegenerateStamina(float deltaTime)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * stamina * deltaTime;
            currentStamina = Mathf.Min(maxStamina, currentStamina);
            OnStaminaChanged?.Invoke(GetStaminaPercent());
        }
    }
    
    private void DecayRage(float deltaTime)
    {
        // Rage decays slowly when not in combat
        if (Time.time - lastDamageTime > 3f)
        {
            currentRage -= 5f * deltaTime;
            currentRage = Mathf.Max(0f, currentRage);
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0f, currentHP);
        lastDamageTime = Time.time;
        
        // Gain rage from taking damage
        GainRage(damage * rageGainPerDamage);
        
        OnHealthChanged?.Invoke(GetHealthPercent());
    }
    
    public void DrainStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(0f, currentStamina);
        OnStaminaChanged?.Invoke(GetStaminaPercent());
    }
    
    public void GainRage(float amount)
    {
        currentRage += amount;
        currentRage = Mathf.Min(maxRage, currentRage);
        OnRageChanged?.Invoke(GetRagePercent());
    }
    
    public void ConsumeRage(float amount)
    {
        currentRage -= amount;
        currentRage = Mathf.Max(0f, currentRage);
        OnRageChanged?.Invoke(GetRagePercent());
    }
    
    public void IncrementCombo()
    {
        comboCount++;
        OnComboCountChanged?.Invoke();
    }
    
    public void ResetCombo()
    {
        comboCount = 0;
        OnComboCountChanged?.Invoke();
    }
    
    // Getters
    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    public float CurrentRage => currentRage;
    public float MaxRage => maxRage;
    public float Speed => speed;
    public float Power => power;
    public float Defense => defense;
    public int ComboCount => comboCount;
    
    public float GetHealthPercent() => currentHP / maxHP;
    public float GetStaminaPercent() => currentStamina / maxStamina;
    public float GetRagePercent() => currentRage / maxRage;
    
    public bool IsAlive => currentHP > 0f;
    public bool CanAttack(float staminaCost) => currentStamina >= staminaCost;
    public bool CanUseUltimate() => currentRage >= maxRage;
}

using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;
    public float CurrentHealth => currentHealth;

    public bool IsDead { get; private set; }
    public Action<float, float> OnHealthChanged;
    public Action Died;
    
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        Debug.Log($"HP: {currentHealth}/{maxHealth}");  // ES -> HP: 100/100
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        Mathf.Clamp(currentHealth, 0f, maxHealth);  // lo setto tra un minimo e un massimo

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        /* OPPURE
        if (currentHealth < maxHealth)
        {
            currentHealth = maxHealth;
        }
        */
    }

    public void SetHealth(float _health, float _maxHealth)
    {
        currentHealth = _health;
        maxHealth = _maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        IsDead = true;
        Died?.Invoke();
        gameObject.SetActive(false);
    }
}

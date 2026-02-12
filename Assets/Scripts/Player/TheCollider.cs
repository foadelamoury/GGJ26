using System;
using UnityEngine;
using UnityEngine;

public class TheCollider : MonoBehaviour, IDamageable
{

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    public event Action<float,float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Die(Collision2D collision)
    {
        // Optional: Handle instant death
        TakeDamage(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth,maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Entity Died: " + gameObject.name);
            OnDeath?.Invoke();
            // Optional: Disable object or trigger explosion
        }
    }
}

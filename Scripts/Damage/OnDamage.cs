using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDamage : MonoBehaviour
{
    // Damage Player
    [SerializeField] private float startingHealth;
    [SerializeField] UnityEvent<float> onHealthPercentageUpdated;
    [SerializeField] private UnityEvent onDestroyed;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void ApplyDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            onDestroyed?.Invoke();
        }

        onHealthPercentageUpdated?.Invoke(startingHealth);
    }
}

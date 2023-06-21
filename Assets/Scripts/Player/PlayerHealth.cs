using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private bool isRolling;

    public HealthController healthBar;

    public UnityEvent OnDeath;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateBar(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }

    public void SetRollingState(bool value)
    {
        this.isRolling = value;
    }

    public void TakeDamage(float damage, bool isCritical)
    {
        if(!isRolling)
        {
            player.PlayerTriggerHurt();
            currentHealth -= damage;
            

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath.Invoke();
            }

            healthBar.UpdateBar(currentHealth, maxHealth);
        }
    }

    public void Death()
    {
        player.PlayerDeath();
    }
}

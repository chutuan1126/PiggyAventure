using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MonsterHeath : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public Monster monster;
    public HealthController healthColltroller;

    public UnityEvent OnDeath;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthColltroller.UpdateBar(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }

    public void TakeDamage(float damage, bool isCritical)
    {
        if (healthColltroller != null)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath.Invoke();
            }
            healthColltroller.ShowDamage(damage, isCritical);
            healthColltroller.UpdateBar(currentHealth, maxHealth);
        }
    }

    public void Death()
    {
        if (healthColltroller != null) Destroy(healthColltroller.gameObject);
        monster.MonsterDeath();
    }
}

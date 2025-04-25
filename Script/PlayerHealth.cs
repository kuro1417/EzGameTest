using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Image playerHealthBar;
    private bool isDead = false;
    private float currentHealth;

    private void Start()
    {
        Instance = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (playerHealthBar == null) return;
        playerHealthBar.fillAmount = currentHealth/maxHealth;

        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public bool IsDead()
    {  
       return isDead;
    }
}

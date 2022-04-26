using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }
}

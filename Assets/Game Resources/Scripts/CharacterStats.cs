using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    
    public int currentHealth { get; private set; }
    public int currentMana { get; private set; }
    public Stat maxHealth;
    public Stat healthRegen;
    public Stat damage;
    public Stat armour;
    public Stat moveSpeed;
    public Stat stamina;
    public Stat staminaRegen;
    public Stat maxMana;
    public Stat manaRegen;

    void Awake ()
    {
        currentHealth = maxHealth.GetValue();
    }

    void Update( )
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
            Debug.Log(currentHealth);
        }
    }

    public void TakeDamage (int damage)
    {
        damage -= armour.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes" + damage + " damage.");

        if (currentHealth <=0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    
    public float currentHealth;
    public float currentMana;

    public CharacterStat maxHealth;
    public CharacterStat healthRegen;
    public CharacterStat damage;
    public CharacterStat armour;
    public CharacterStat moveSpeed;
    public CharacterStat stamina;
    public CharacterStat staminaRegen;
    public CharacterStat maxMana; 
    public CharacterStat manaRegen;

    void Start ()
    {
        currentHealth = maxHealth.Value;
        currentMana = maxMana.Value;
    }

    void Update( )
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
            Debug.Log(currentHealth);
        }
    }

    public void TakeDamage (float damage)
    {
        damage -= armour.Value;
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

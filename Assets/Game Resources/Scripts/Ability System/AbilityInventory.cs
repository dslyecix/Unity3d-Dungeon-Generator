using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInventory : MonoBehaviour {

    public const int numAbilitySlots = 6;

    public Image[] abilityImages = new Image[numAbilitySlots];
    public Ability[] abilities = new Ability[numAbilitySlots];

    public void AddAbility(Ability abilityToAdd)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] == null)
            {
                abilities[i] = abilityToAdd;
                abilityImages[i].sprite = abilityToAdd.sprite;
                abilityImages[i].enabled = true;
                return;
            } 
        }

        Debug.Log("No room for " + abilityToAdd.name);
    }

    public void RemoveAbility(Ability abilityToRemove)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] == abilityToRemove)
            {
                abilities[i] = null;
                abilityImages[i].sprite = null;
                abilityImages[i].enabled = false;
                return;
            }
        }
        
        Debug.Log("No room for " + abilityToRemove.name);
    }	
}

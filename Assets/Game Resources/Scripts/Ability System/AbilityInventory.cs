using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInventory : MonoBehaviour {

    public const int numbAbilitySlots = 6;

    public Image[] abilityImages = new Image[numbAbilitySlots];
    public Ability[] abilities = new Ability[numbAbilitySlots];

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
    }
	
}

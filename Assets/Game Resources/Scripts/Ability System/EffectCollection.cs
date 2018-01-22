using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCollection : MonoBehaviour {

	public Effect[] effects = new Effect[0];

    public void ExecuteEffects ()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Execute();
        }
    }
}

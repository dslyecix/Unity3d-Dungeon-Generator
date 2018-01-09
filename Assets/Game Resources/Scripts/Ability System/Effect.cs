using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effect : ScriptableObject
{
    public new string name { get; set; }

	public List<Effect> subEffects;

	public abstract void Execute(GameObject source, ref List<GameObject> targets);

	public void ExecuteEffects(GameObject source, ref List<GameObject> targets)
	{
        if (subEffects.Count > 0)
        {
            Debug.Log("Executing subeffects");
            foreach (var effect in subEffects)
            {
                effect.Execute(source, ref targets);
            }
        }
	}
}

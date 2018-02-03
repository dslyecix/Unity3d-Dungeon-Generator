using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effect : ScriptableObject
{
    public new string name { get; set; }

    public ConditionCollection conditionCollection;
	public EffectCollection effectCollection;

	public abstract void Execute(GameObject source, ref List<GameObject> targets);

	public void ExecuteSubEffects(GameObject source, ref List<GameObject> targets)
	{
        if (effectCollection != null) 
        {
            if (effectCollection.effects.Length > 0)
            {
                if (conditionCollection.CheckConditions())
                    {
                    foreach (var effect in effectCollection.effects)
                    {
                        effect.Execute(source, ref targets);
                        Debug.Log("Executing " + effect.name);
                    }
                }
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effect : ScriptableObject
{
	// Sub-effects are the effects to be executed (in order) by this effect
	public List<Effect> subEffects;

	public abstract void Execute(GameObject source, List<GameObject> targets);

	public void ExecuteSubEffects(GameObject source, List<GameObject> targets)
	{
		foreach (var effect in subEffects)
		{
			effect.Execute(source, targets);
		}
	}
}

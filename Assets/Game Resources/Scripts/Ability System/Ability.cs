using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/New Ability")]
public class Ability : Effect
{
	public new string name;
	public int manaCost;

    public override void Execute(GameObject source, List<GameObject> targets)
    {
		ExecuteSubEffects(source, targets);
    }

}

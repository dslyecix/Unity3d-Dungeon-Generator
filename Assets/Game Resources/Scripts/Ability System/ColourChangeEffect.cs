using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/Colour Change")]
public class ColourChangeEffect : Effect
{
	public enum Length { _1s = 1,
	 					_2s = 2,
						_5s = 5,
						_10s = 10 }

	public Length length;

    public Color color;

    public override void Execute(GameObject source, ref List<GameObject> targets)
    {
		foreach (var target in targets)
		{
			Renderer renderer = target.GetComponent<Renderer>();
			if (renderer != null)
			{
                renderer.material.color = color;
				//ActiveSpellManager.instance.StartCoroutine(ColourCoroutine(renderer));
			}
		}

        ExecuteSubEffects(source, ref targets);        
    }
}

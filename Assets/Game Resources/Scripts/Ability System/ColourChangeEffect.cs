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

    public override void Execute(GameObject source, List<GameObject> targets)
    {
		foreach (var target in targets)
		{
			Renderer renderer = target.GetComponent<Renderer>();
			if (renderer != null)
			{
				ActiveSpellManager.instance.StartCoroutine(ColorCoroutine(renderer));
			}
		}

        ExecuteSubEffects(source, targets);
    }

	public IEnumerator ColorCoroutine(Renderer renderer)
	{	
		Material oldMat = renderer.material;
		Material newMat = renderer.material;
		newMat.color = Color.red;
		renderer.material = newMat;

		yield return new WaitForSeconds((float)length);

		renderer.material = oldMat;
		ActiveSpellManager.instance.StopCoroutine(ColorCoroutine(renderer));
	}
}

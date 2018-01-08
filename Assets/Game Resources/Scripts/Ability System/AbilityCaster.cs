using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour {

    public List<GameObject> targets;

	public IEffect effect
	{
		get { return _container.Result; }
		set { _container.Result = value; }
	}
    
	[SerializeField]
	private IEffectContainer _container;

	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("P pressed");
			
			effect.Apply(this.gameObject, targets);

		}
	}
}

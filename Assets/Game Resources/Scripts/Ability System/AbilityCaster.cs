using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour {

	public Ability ability;

    public List<GameObject> targets;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("P pressed");
			
			ability.Execute(this.gameObject, targets);

		}
	}
}

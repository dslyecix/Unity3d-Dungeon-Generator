using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour {

	public List<Ability> abilities;

    public List<GameObject> targets;
    private List<GameObject> originalTargets;

    void Start () {
        originalTargets = targets;
    }


	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("P pressed");
			
			abilities[0].Execute(this.gameObject, ref targets);
            targets = originalTargets;
		}

        if (Input.GetKeyDown(KeyCode.O))
		{
			Debug.Log("O pressed");
			
			abilities[1].Execute(this.gameObject, ref targets);
             targets = originalTargets;
		}
	}
}

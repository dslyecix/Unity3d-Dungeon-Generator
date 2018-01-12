using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour {

	public List<Ability> abilities;

    public List<GameObject> targets;
    private List<GameObject> originalTargets = new List<GameObject>();

    void Start () {
        originalTargets.AddRange(targets);
    }

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			abilities[0].Execute(this.gameObject, ref targets);
            ClearTargets();
		}

        if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			abilities[1].Execute(this.gameObject, ref targets);
            ClearTargets();
		}
	}

    void ClearTargets()
    {
        targets.Clear();
        targets.AddRange(originalTargets);
    }
}

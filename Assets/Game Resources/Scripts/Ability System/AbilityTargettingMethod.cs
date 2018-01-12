using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbilityTargettingMethod : ScriptableObject {

    public GameObject source;
    public List<GameObject> targets;

	public abstract void Execute ();

}

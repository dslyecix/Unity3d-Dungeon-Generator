using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/Get Targets in Radius")]
public class GetTargetsInRadius : Effect
{
	public enum Radius { _1 = 1,
                            _5 = 5,
                            _10 = 10,
                            _20 = 10,
                            _30 = 30,
                            _40 = 40,
                            _50 = 50, }
	
	public Radius radius;

    public override void Execute(GameObject source, ref List<GameObject> targets)
    {
        Debug.Log("Executing Get Targets in Radius Effect");

        List<GameObject> temporaryTargets = new List<GameObject>();

        foreach (var target in targets)
        {
            Collider[] newTargets = Physics.OverlapSphere(target.transform.position, (float)radius);

            for (int i = 0; i < newTargets.Length; i++)
            {
                GameObject newGO = newTargets[i].gameObject;
                if (!temporaryTargets.Contains(newGO) && !targets.Contains(newGO))
                {
                    temporaryTargets.Add(newTargets[i].gameObject);
                }   
            }        
        }
        
        targets.AddRange(temporaryTargets);

        ExecuteEffects(source, ref targets);
        
    }

}
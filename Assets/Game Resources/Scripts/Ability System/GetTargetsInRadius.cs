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
        temporaryTargets = new List<GameObject>(targets);

        foreach (var target in targets)
        {
            Collider[] newTargets = Physics.OverlapSphere(source.transform.position, (float)radius);

            List<GameObject> newTargetGOs = new List<GameObject>();

            for (int i = 0; i < newTargets.Length; i++)
            {
                GameObject currentGO = newTargets[i].gameObject;
                if(!newTargetGOs.Contains(currentGO))
                {
                    
                    newTargetGOs.Add(newTargets[i].gameObject);
                }
            }

            foreach (var newTargetGO in newTargetGOs)
            {
                if (!temporaryTargets.Contains(newTargetGO))
                {
                    Debug.Log(newTargetGO.name + "is not in list of existing targets");
                    temporaryTargets.Add(newTargetGO);
                }   
            }
        }
        targets.Clear();
        targets = temporaryTargets;
        ExecuteEffects(source, ref targets);
    }
}

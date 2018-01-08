using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AuraEffect : ScriptableObject, IEffect
{

	public enum AuraRadius { _1 = 1,
                            _5 = 5,
                            _10 = 10,
                            _20 = 10,
                            _30 = 30,
                            _40 = 40,
                            _50 = 50, }

	private readonly IEffect baseEffect;

	public AuraRadius auraRadius;

    public AuraEffect(IEffect baseEffect)
    {
		this.baseEffect = baseEffect;
    }

    public IEffect nextEffect
        {
            get { return _nextEffect.Result; }
            set { _nextEffect.Result = value; }
        }
    
	[SerializeField]
	private IEffectContainer _nextEffect;

    public void Apply(GameObject source, List<GameObject> targets)
    {
        List<GameObject> temporaryTargets = new List<GameObject>();
        temporaryTargets = targets;

        foreach (var target in targets)
        {
            Collider[] newTargets = Physics.OverlapSphere(source.transform.position, (float)auraRadius);
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
                    temporaryTargets.Add(newTargetGO);
                }   
            }
        }

        if(nextEffect != null) nextEffect.Apply(source, temporaryTargets);
    }
}

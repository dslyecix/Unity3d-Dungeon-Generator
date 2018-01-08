using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KnockbackEffect : ScriptableObject, IEffect
{

	public enum KnockbackDirection { forward, backwards, upwards, towards, away }

	private readonly IEffect baseEffect;

	public KnockbackDirection knockbackDirection;
	public float force;

    public KnockbackEffect(IEffect baseEffect)
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
        foreach (var target in targets)
        {        
            Debug.Log("Applying effect");
			//baseEffect.Apply(source, target);
			Rigidbody rigidbody = target.GetComponent<Rigidbody>();
			if (rigidbody) {
                
                Vector3 dir;
                switch (knockbackDirection)
                {
                    case KnockbackDirection.forward: 
                        dir = target.transform.forward;
                        break;
                    case KnockbackDirection.backwards: 
                        dir = -target.transform.forward;  
                        break;
                    case KnockbackDirection.upwards: 
                        dir = target.transform.up;   
                        break;
                    case KnockbackDirection.towards: 
                        dir = (source.transform.position - target.transform.position).normalized;
                        break;
                    case KnockbackDirection.away: 
                        dir = (target.transform.position - source.transform.position).normalized;
                        break;
                    default:
                        dir = Vector3.zero;
                        break;
                }
            
                Debug.Log("Adding force in " + knockbackDirection + " !");
				rigidbody.AddForce(dir * force);
			}
        }

        if(nextEffect != null) nextEffect.Apply(source, targets);
    }
}

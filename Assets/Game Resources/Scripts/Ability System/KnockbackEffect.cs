using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Abilities/Effects/Knockback Effect")]
public class KnockbackEffect : Effect
{
    public enum KnockbackDirection { forward, backwards, upwards, towards, away }
	[SerializeField] private KnockbackDirection knockbackDirection;

	public float force;

    public enum AffectedTag { Object }
    [SerializeField] private AffectedTag tag;

    public override void Execute(GameObject source, ref List<GameObject> targets)
    {
        Debug.Log("Executing Knockback Effect");

        foreach (var target in targets)
        {        
            if (target.tag == tag.ToString())
            {
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
        }

        ExecuteEffects(source, ref targets);
    }
}

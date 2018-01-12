using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Abilities/Effects/WaitForTime")]
public class DelayTimeEffect : Effect
{
    public enum Length { _1s = 1,
	 					_2s = 2,
						_5s = 5,
						_10s = 10 }

	public Length length;

    float startTime;
    float endTime;


    public override void Execute(GameObject source, ref List<GameObject> targets)
    {
        startTime = Time.time;
        endTime = startTime + (float)length;
        Debug.Log("Waiting for " + length + " seconds!");

       // ActiveSpellManager.Instance.StartCoroutine(Wait((float)length));
       
    }

    // IEnumerator Wait(float time)
    // {
    //     yield return 
    // }

}

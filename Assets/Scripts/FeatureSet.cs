using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FeatureSet : ScriptableObject {

	public Feature.Type Type;

	public List<GameObject> features;

	public GameObject ReturnRandomFeature() 
	{
		// DRandom rand = new DRandom();
        // features.Shuffle(rand.random);
		// for (int i = 0; i < features.Count; i++) {
        //     if (features[i]) return features[i];
        // }
        return features[0];
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BiomeFeatureSets : ScriptableObject {

	public DungeonType.Biome biome;

	public FeatureSet floorFeatures; 
	public FeatureSet wallFeatures;
	// public FeatureSet ceilingFeatures;
	// public FeatureSet rampFeatures; 
	public FeatureSet stairFeatures; 
	public FeatureSet pillarFeatures; 
	// public FeatureSet doorFeatures;

    public GameObject ReturnFeature(Feature.Type type)
    {

		switch (type)
		{
			// case Feature.Type.Ceiling: return ceilingFeatures.ReturnRandomFeature();
			// case Feature.Type.Door: return doorFeatures.ReturnRandomFeature();
			case Feature.Type.Floor: return floorFeatures.ReturnRandomFeature();
			case Feature.Type.Pillar: return pillarFeatures.ReturnRandomFeature();
			// case Feature.Type.Ramp: return rampFeatures.ReturnRandomFeature();
			case Feature.Type.Stair: return stairFeatures.ReturnRandomFeature();
			case Feature.Type.Wall: return wallFeatures.ReturnRandomFeature();
			default: return null;
		}
    }


    // public Feature GetRandomFeature(Feature.Type type, DRandom random) {		
    //     switch (type)
    // 	{

    // 		case Feature.Type.Floor:
    // 			floorFeatures.Shuffle(random.random);
    // 			floorFeatures[0].features.Shuffle(random.random);
    // 			return floorFeatures[0].features[0];

    // 		case Feature.Type.Wall:
    // 			wallFeatures.Shuffle(random.random);
    // 			wallFeatures[0].features.Shuffle(random.random);
    // 			return wallFeatures[0].features[0];

    // 		default:
    // 			break;
    // 	}

    // }

}

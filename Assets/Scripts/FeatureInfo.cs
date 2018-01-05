using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureInfo {

	public Vector3 position;
	public Vector3 direction;
	public Feature.Type type;

    public FeatureInfo (Vector3 _pos, Vector3 _dir, Feature.Type _type)
	{
		position = _pos;
		direction = _dir;
		type = _type;
	}

}

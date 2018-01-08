using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Feature : MonoBehaviour {

	public enum Type { Wall, Floor, Door, Ceiling, Stair, Ramp, Pillar, }

	public Type type;
	
	private Vector3 position;
	private Vector3 direction;

    public Vector3 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

}

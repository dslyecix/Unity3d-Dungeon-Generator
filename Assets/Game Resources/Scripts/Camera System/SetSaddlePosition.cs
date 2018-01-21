using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaddlePosition : MonoBehaviour {

    Vector3 originalOffset;
    Vector3 newPos;
	// Use this for initialization
	void Start () {
		originalOffset = transform.position - transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
        newPos = new Vector3(transform.parent.position.x, originalOffset.y, transform.parent.position.z + originalOffset.z);

		transform.position = newPos;
	}
}

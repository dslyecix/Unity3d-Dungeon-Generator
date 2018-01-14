﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour {

	Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPosition;
            Rigidbody rb = this.GetComponent<Rigidbody>();
            if (rb) rb.velocity = Vector3.zero;
        }
	}
}

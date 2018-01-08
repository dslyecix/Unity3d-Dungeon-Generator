using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsTranslator : MonoBehaviour {

    public GameObject forceDetectionRigidbodyContainer;

    private PlayerCharacterController controller;
    private Rigidbody forceRigidbody;

	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerCharacterController>();
        forceRigidbody = forceDetectionRigidbodyContainer.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (forceRigidbody.velocity.magnitude != 0)
        {
            controller.AddVelocity(forceRigidbody.velocity * Time.deltaTime);
        }
	}
}

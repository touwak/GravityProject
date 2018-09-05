using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    [HideInInspector]
    public float movementSpeed;
        
    Vector3 movementForce;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        movementForce = new Vector3(movementSpeed, 0, 0);

        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        movementForce *= (Time.deltaTime * 60);

        rb.AddForce(movementForce);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [Range(30f, 100f)]
    [Tooltip("The force of the gravity")]
    public float gravityForce;

	// Use this for initialization
	void Start () {
        GetComponent<ConstantForce>().force = new Vector3(0, gravityForce, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

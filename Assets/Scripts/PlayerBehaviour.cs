using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [Range(10f, 50f)]
    [Tooltip("The force of the gravity")]
    public float gravityForce;

    private ConstantForce cf;
    
	// Use this for initialization
	void Start () {
        cf = GetComponent<ConstantForce>();
        cf.force = new Vector3(0, gravityForce * -1f, 0);
    }
	
	// Update is called once per frame
	void Update () {
        DetectInput();
	}

    void DetectInput() {
        //Check if we are running either in the Unity editor or in a
        //standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER      
        // If the mouse is held down (or the screen is tapped
        // on Mobile)
        if (Input.GetMouseButton(0)) {
            ChangeGravity();
        }
        //Check if we are running on a mobile device
#elif UNITY_IOS || UNITY_ANDROID
        // Check if Input has registered more than zero touches
        if (Input.touchCount > 0) {
            
            ChangeGravity();
        }
#endif
    }

    void ChangeGravity() {

        gravityForce *= -1;
        cf.force = new Vector3(0, gravityForce, 0);
    }
}

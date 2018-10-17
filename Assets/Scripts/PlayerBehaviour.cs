using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    [Range(1f, 10f)]
    [Tooltip("The force of the gravity")]
    public float gravityForce;

    private ConstantForce cf;

    //score
    private float score = 0;
    public Text scoreText;

    // Use this for initialization
    void Start () {
        cf = GetComponent<ConstantForce>();
        cf.force = new Vector3(0, gravityForce * -1f, 0);

        score = 0;
    }
	
	// Update is called once per frame
	void Update () {

        // If the game is paused, don't do anything
        if (PauseScreenBehaviour.paused) {
            return;
        }

        Score += Time.deltaTime;

        DetectInput();
	}

    /// <summary>
    /// detect the player input
    /// </summary>
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

    /// <summary>
    /// Change the player's gravity
    /// </summary>
    void ChangeGravity() {
        gravityForce *= -1;
        cf.force = new Vector3(0, gravityForce, 0);
    }

    

    public float Score {
        get { return score; }
        set {
            score = value;
            
            // Update the text to display the whole number portion
            // of the score
            scoreText.text = string.Format("{0:0}", score);

            //Set High Score
            if(score > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", (int)score);
            }
        }
    }
}

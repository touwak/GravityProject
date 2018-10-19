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
    public Text scoreTextGO;
    public Text highScoreTextGO;

    public GameController gameController;

    //particles
    ParticleSystem particle;
    Renderer particleRender;

    // Use this for initialization
    void Start () {
        //constant force
        cf = GetComponent<ConstantForce>();
        gravityForce *= -1f;
        cf.force = new Vector3(0, gravityForce, 0);

        //particles
        particle = GetComponentInChildren<ParticleSystem>();
        particleRender = particle.GetComponent<Renderer>();

        //score
        score = 0;
        highScoreTextGO.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
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
//#if UNITY_STANDALONE || UNITY_WEBPLAYER      
        // If the mouse is held down (or the screen is tapped
        // on Mobile)
        if (Input.GetMouseButtonDown(0)) {
            ChangeGravity();
        }
        //Check if we are running on a mobile device
//#elif UNITY_IOS || UNITY_ANDROID
        // Check if Input has registered more than zero touches
        if (Input.touchCount > 0 && Input.touchCount < 2) {            
            ChangeGravity();
        }
//#endif
    }

    /// <summary>
    /// Change the player's gravity
    /// </summary>
    void ChangeGravity() {

        gravityForce *= -1;
        cf.force = new Vector3(0, gravityForce, 0);

        if (gravityForce > 0) {
            particleRender.material.color = Color.red;
        }
        else {
            particleRender.material.color = Color.blue;
        }

    }


    private int lastIncrement = 0;

    public float Score {
        get { return score; }
        set {
            score = value;
            
            // Update the text to display the whole number portion
            // of the score
            scoreTextGO.text = scoreText.text = string.Format("{0:0}", score);

            //increment the difficult
            if(lastIncrement < (int)score && 
                score > 1f && 
                (int)score % gameController.difficultThreshold == 0) {

                lastIncrement = (int)score;
                gameController.IncrementDifficult();
            }

            //Set High Score
            if(score > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", (int)score);
                highScoreTextGO.text = string.Format("{0:0}", score);
            }
        }
    }
}

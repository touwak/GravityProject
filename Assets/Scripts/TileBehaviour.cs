using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    //seted in the game controller
    [HideInInspector]
    public float movementSpeed;
    
    public Vector3 movementForce;

    public GameObject topTile;
    public GameObject bottomTile;

    public float spaceBetweenTilesY;
    public float effectSpeed = 1.0f;

    [HideInInspector]
    public bool separateEffectEnable = false;

    Rigidbody rb;
    Renderer topTileRenderer;
    Renderer bottomTileRenderer;

    //spawn effect
    Vector3 startTilePos;
    Vector3 endTilePos;
    float startTime;
    float topJourneyLength;

    //reference to the game controller
    private GameController gameController;

    void Start () {
        movementForce = new Vector3(movementSpeed, 0, 0);
        rb = GetComponent<Rigidbody>();
        spaceBetweenTilesY = FindObjectOfType<GameController>().spaceBetweenTilesY;

        topTileRenderer = topTile.GetComponent<Renderer>();
        bottomTileRenderer = bottomTile.GetComponent<Renderer>();

        gameController = FindObjectOfType<GameController>();

        if (separateEffectEnable) {
            SetSepareteEffect();
        }

        ChangeColor();
    }

    private void OnEnable() {
        if (separateEffectEnable) {
            SetSepareteEffect();
        }

        ChangeColor();
    }

    void Update () {
        movementForce *= (Time.deltaTime * 60);
        rb.AddForce(movementForce);

        if (separateEffectEnable) {
            SeparateEffect();
        }
	}

    #region SEPARATE EFFECT

    /// <summary>
    /// set the necessary variables for the separate effect
    /// </summary>
    void SetSepareteEffect() {
        if (topTile && bottomTile) {
            
            //spawn effect
            startTilePos = topTile.transform.localPosition;
            startTilePos.y = topTile.transform.localScale.y * 0.5f;

            endTilePos = startTilePos;
            endTilePos.y = spaceBetweenTilesY;

            topJourneyLength = Vector3.Distance(startTilePos, endTilePos);

            startTime = Time.time;
            separateEffectEnable = true;
        }
    }

    /// <summary>
    /// Separate the top and bottom tile 
    /// </summary>
    void SeparateEffect() {          
        // distance moved = time * speed
        float distCovered = (Time.time - startTime) * effectSpeed;

        // check if the effect has already reach the final point
        if (distCovered > topJourneyLength) {
            separateEffectEnable = false;
            return;
        }

        // fraction of journey completed = current distance / total distance
        float fracJourney = distCovered / topJourneyLength;
       
        topTile.transform.localPosition = Vector3.Lerp(startTilePos, endTilePos, fracJourney);
        bottomTile.transform.localPosition = Vector3.Lerp(startTilePos * -1.0f, endTilePos * -1.0f, fracJourney);
    }
    #endregion

    void ChangeColor() {
        if (topTileRenderer != null) {
            Color color = topTileRenderer.material.color;
            color.g = gameController.GetColorIterator();
            topTileRenderer.material.color = color;
        }

        //lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
    }
}

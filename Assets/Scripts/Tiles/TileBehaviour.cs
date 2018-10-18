﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileBehaviour : MonoBehaviour {

    public float effectSpeed = 1.0f;

    [HideInInspector]
    public bool separateEffectEnable = false;

    [SerializeField]   
    private GameObject topTile;
    [SerializeField]
    private GameObject bottomTile;

    private float spaceBetweenTilesY;
    private float movementSpeed;

    Rigidbody rb;
    Renderer topTileRenderer;
    Renderer bottomTileRenderer;
   
    //spawn effect
    Vector3 startTilePos;
    Vector3 endTilePos;
    float startTime;
    float topJourneyLength;

    //Color
    Color secondColor;

    GameObject player;
    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 0.5f;

    GameController gc;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start () {
       
        topTileRenderer = topTile.GetComponent<Renderer>();
        bottomTileRenderer = bottomTile.GetComponent<Renderer>();

        if (separateEffectEnable) {
            SetSepareteEffect();
        }      

        ChangeColor(topTileRenderer);
        ChangeColor(bottomTileRenderer);
    }


    void OnEnable() {
        if (separateEffectEnable) {
            SetSepareteEffect();
        }

        ChangeColor(topTileRenderer);
        ChangeColor(bottomTileRenderer);
    }

    void Update () {

        if (separateEffectEnable) {
            SeparateEffect();
        }
	}

    /// <summary>
    /// Set the speed movement of the tile
    /// </summary>
    /// <param name="speed"> tile speed </param>
    public void SetVelocity(float speed) {
        if (speed > 0) {
            movementSpeed = speed;
        }

        rb.velocity = new Vector3(speed * -1f, 0, 0);
    }

    /// <summary>
    /// Set the space between top and bottom tile
    /// </summary>
    /// <param name="separation">space between tiles</param>
    public void SetSpaceBetweenTilesY(float separation) {
        spaceBetweenTilesY = separation;
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

    #region COLORS
    /// <summary>
    /// Set the second color of the color lerp
    /// </summary>
    /// <param name="color">new color</param>
    public void SetSecondColor(Color color) {
        secondColor = color;
    }

    /// <summary>
    /// Change the material color
    /// </summary>
    /// <param name="renderer">render that contains the material</param>
    void ChangeColor(Renderer renderer) {
        if (renderer != null) {
            renderer.material.color = Color.Lerp(Color.black, secondColor, Mathf.PingPong(Time.time, 1));
        }
    }
    #endregion

    #region PLAYER DIE AND GAME OVER

    private void OnCollisionEnter(Collision collision) {

        if(collision.gameObject.GetComponent<PlayerBehaviour>()) {

            gc = FindObjectOfType<GameController>();

            //keep moving the tile in the same direction
            //SetVelocity(movementSpeed);
            StopOrRestartAllTiles(true);

            // Destroy (Hide) the player
            collision.gameObject.SetActive(false);
            player = collision.gameObject;

            
            // Call the function ResetGame after waitTime
            // has passed
            Invoke("ResetGame", waitTime);
        }
    }

    /// <summary>
    /// Will restart the currently loaded level
    /// </summary>
    void ResetGame() {
        //Bring up restart menu
        var go = GetGameOverMenu();
        go.SetActive(true);

        PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
        playerBehaviour.scoreText.gameObject.SetActive(false);
       

        // Get our continue button
        var buttons = go.transform.GetComponentsInChildren<Button>();
        UnityEngine.UI.Button continueButton = null;

        foreach (var button in buttons) {
            if (button.gameObject.name == "Continue Button") {
                continueButton = button;
                break;
            }
        }

        if (continueButton) {
            #if UNITY_ADS
            // If player clicks on button we want to play ad and
            // then continue
            StartCoroutine(ShowContinue(continueButton));
            UnityAdController.tile = this;
            #else
            // If can't play an ad, no need for continue button
            continueButton.gameObject.SetActive(false);
            #endif
        }
    }

    /// <summary>
    /// Handles resetting the game if needed
    /// </summary>
    public void Continue() {
        var go = GetGameOverMenu();
        go.SetActive(false);

        //TODO move the player
        player.transform.position = transform.position;
        player.SetActive(true);

        StopOrRestartAllTiles(false);
    }

    /// <summary>
    /// Retrieves the Game Over menu game object
    /// </summary>
    /// <returns>The Game Over menu object</returns>
    GameObject GetGameOverMenu() {
        return GameObject.Find("Canvas").transform.Find("Game Over").gameObject;
    }

    void StopOrRestartAllTiles(bool stop) {     

        if (stop) {
            gc.SetTilesSpeed(0);
        }
        else {
            gc.SetTilesSpeed(movementSpeed);
        }
    }

    public IEnumerator ShowContinue(UnityEngine.UI.Button contButton) {

        while (true) {
            var btnText = contButton.GetComponentInChildren<Text>();
            
            // Check if we haven't reached the next reward time yet
            // (if one exists)
            if (UnityAdController.nextRewardTime.HasValue &&
                (DateTime.Now < UnityAdController.nextRewardTime.Value)) {

                // Unable to click on the button
                contButton.interactable = false;
                
                // Get the time remaining until we get to the next
                // reward time
                TimeSpan remaining = UnityAdController.nextRewardTime.Value
                - DateTime.Now;
                
                // Get the time left in the following format 99:99
                var countdownText = string.Format("{0:D2}:{1:D2}",
                remaining.Minutes,
                remaining.Seconds);

                // Set our button's text to reflect the new time
                btnText.text = countdownText;
                
                // Come back after 1 second and check again
                yield return new WaitForSeconds(1f);
            }
            else if (!UnityAdController.showAds) {
                // It's valid to click the button now
                contButton.interactable = true;
                
                // If player clicks on button we want to just continue
                contButton.onClick.AddListener(Continue);

                UnityAdController.tile = this;
                
                // Change text to allow continue
                btnText.text = "Free Continue";
                
                // We can now leave the coroutine
                break;
            }
            else {
                // It's valid to click the button now
                contButton.interactable = true;
                
                // If player clicks on button we want to play ad and
                // then continue
                contButton.onClick.AddListener(UnityAdController.ShowRewardAd);

                UnityAdController.tile = this;
                
                // Change text to its original version
                btnText.text = "Continue (Play Ad)";
                
                // We can now leave the coroutine
                break;
            }
        }
    }

    #endregion
}
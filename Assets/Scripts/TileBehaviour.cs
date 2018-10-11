﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    public float effectSpeed = 1.0f;

    [HideInInspector]
    public bool separateEffectEnable = false;

    [SerializeField]   
    private GameObject topTile;
    [SerializeField]
    private GameObject bottomTile;

    private float spaceBetweenTilesY;
    private Vector3 movementForce;

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

    void Start () {
       
        rb = GetComponent<Rigidbody>();

        topTileRenderer = topTile.GetComponent<Renderer>();
        bottomTileRenderer = bottomTile.GetComponent<Renderer>();

        if (separateEffectEnable) {
            SetSepareteEffect();
        }      

        ChangeColor(topTileRenderer);
        ChangeColor(bottomTileRenderer);
    }


    private void OnEnable() {
        if (separateEffectEnable) {
            SetSepareteEffect();
        }

        ChangeColor(topTileRenderer);
        ChangeColor(bottomTileRenderer);
    }

    void Update () {
        movementForce *= (Time.deltaTime * 60);
        rb.AddForce(movementForce);

        if (separateEffectEnable) {
            SeparateEffect();
        }
	}

    /// <summary>
    /// Set the speed movement of the tile
    /// </summary>
    /// <param name="movementSpeed"> speed </param>
    public void SetMovementSpeed(float movementSpeed) {
        movementForce = new Vector3(movementSpeed, 0, 0);
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

    //Color RandColor() {
    //    int i = Random.Range(0, colors.Length);

    //    return colors[i];
    //}

    //public void SetColors(ref Color first, ref Color second) {
    //    first = RandColor();
    //    second = RandColor();

    //    while (second == first) {
    //        second = RandColor();
    //    }
    //}

    //public void SetColor(ref Color color) {
    //    color = RandColor();        
    //}

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
            renderer.material.color = Color.Lerp(Color.black, secondColor, Mathf.PingPong(Time.time, 1)); ;
        }
    }

    #endregion
}

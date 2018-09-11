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

    Rigidbody rb;

    //spawn effect
    Vector3 topTilePos;
    Vector3 topTilePosEnd;
    Vector3 bottomTilePos;
    Vector3 bottomTilePosEnd;
    float startTime;
    float topJourneyLength;
    float bottomJourneyLength;
    bool separateEffectEnable;

    void Start () {
        movementForce = new Vector3(movementSpeed, 0, 0);
        rb = GetComponent<Rigidbody>();
        spaceBetweenTilesY = FindObjectOfType<GameController>().spaceBetweenTilesY;

        PrepareSepareteEffect();
    }

    private void OnEnable() {
        PrepareSepareteEffect();
    }

    void Update () {
        movementForce *= (Time.deltaTime * 60);

        rb.AddForce(movementForce);

        if (separateEffectEnable) {
            SeparateEffect();
        }
	}

    float CalcDistanceBetweenPoints(Vector3 start, Vector3 end) {
        return Vector3.Distance(start, end);            
    }

    void PrepareSepareteEffect() {
        if (topTile && bottomTile) {
            //spawn effect
            //top
            topTilePos = topTile.transform.localPosition;
            topTilePos.y = topTile.transform.localScale.y * 0.5f;

            topTilePosEnd = topTilePos;
            topTilePosEnd.y = spaceBetweenTilesY;

            topJourneyLength = CalcDistanceBetweenPoints(topTilePos, topTilePosEnd);

            //end
            bottomTilePos = bottomTile.transform.localPosition;
            bottomTilePos.y = (bottomTile.transform.localScale.y * 0.5f) * -1.0f;

            bottomTilePosEnd = bottomTilePos;
            bottomTilePosEnd.y = spaceBetweenTilesY * -1.0f;

            bottomJourneyLength = CalcDistanceBetweenPoints(bottomTilePos, bottomTilePosEnd);

            topTile.transform.localPosition = topTilePos;
            bottomTile.transform.localPosition = bottomTilePos;

            startTime = Time.time;
            separateEffectEnable = true;
        }
    }

    void SeparateEffect() {          
        // distance moved = time * speed
        float distCovered = (Time.time - startTime) * effectSpeed;

        if (distCovered > topJourneyLength) {
            separateEffectEnable = false;
            return;
        }

        // fraction of journey completed = current distance / total distance
        float fracJourney = distCovered / topJourneyLength;
       
        topTile.transform.localPosition = Vector3.Lerp(topTilePos, topTilePosEnd, fracJourney);
        bottomTile.transform.localPosition = Vector3.Lerp(bottomTilePos, bottomTilePosEnd, fracJourney);
    }
}

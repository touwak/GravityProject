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

    void Start () {
        movementForce = new Vector3(movementSpeed, 0, 0);
        rb = GetComponent<Rigidbody>();
        spaceBetweenTilesY = FindObjectOfType<GameController>().spaceBetweenTilesY;

        if (topTile && bottomTile) {
            //spawn effect
            //top
            topTilePos = topTile.transform.position;
            topTilePos.y = topTile.transform.localScale.y * 0.5f;

            topTilePosEnd = topTilePos;
            topTilePosEnd.y = spaceBetweenTilesY;

            topJourneyLength = CalcDistanceBetweenPoints(topTilePos, topTilePosEnd);

            //end
            bottomTilePos = bottomTile.transform.position;
            bottomTilePos.y = (bottomTile.transform.localScale.y * 0.5f) * -1.0f;

            bottomTilePosEnd = bottomTilePos;
            bottomTilePosEnd.y = spaceBetweenTilesY * -1.0f;

            bottomJourneyLength = CalcDistanceBetweenPoints(bottomTilePos, bottomTilePosEnd);

            topTile.transform.position = topTilePos;
            bottomTile.transform.position = bottomTilePos;

            startTime = Time.time;          
        }
    }

	void Update () {
        movementForce *= (Time.deltaTime * 60);

        rb.AddForce(movementForce);

        SeparateEffect();
	}

    float CalcDistanceBetweenPoints(Vector3 start, Vector3 end) {
        return Vector3.Distance(start, end);            
    }

    void SeparateEffect() {
        // distance moved = time * speed
        float distCovered = (Time.time - startTime) * effectSpeed;

        // fraction of journey completed = current distance / total distance
        float fracJourney = distCovered / topJourneyLength;

        topTile.transform.position = Vector3.Lerp(topTilePos, topTilePosEnd, fracJourney);
        topTile.transform.position = Vector3.Lerp(bottomTilePos, bottomTilePosEnd, fracJourney);
    }
}

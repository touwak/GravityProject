using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameController : MonoBehaviour {

    [Tooltip("A reference to the tile we want to spawn")]
    public GameObject tile;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(-5, -9, -3);

    [Tooltip("How many tiles should we create for the pool")]
    [Range(1, 40)]
    public int initPoolNum = 40;

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 30)]
    public int initSpawnNum = 10;

    [Tooltip("Space between the last tile and the new one")]
    [Range(1, 2)]
    public float spaceBetweenTiles = 1.1f;

    [Tooltip("The speed of the tiles")]
    [Range(-1, 0)]
    public float tileMovementSpeed = -0.16f;

    [Tooltip("The min position in the Y")]
    public float tileMinY = -4.2f;

    [Tooltip("The max position in the Y")]
    public float tileMaxY = -0.9f;

    /// <summary>
    /// Where the next tile should be spawned at.
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// How should the next tile be rotated?
    /// </summary>
    private Quaternion nextTileRotation;

    /// <summary>
    /// pool of tiles
    /// </summary>
    private List<GameObject> tiles;

    private GameObject lastTile;
    private bool settingTheStage;
    private Vector3[] bezierPoints;
    private int bezierDivisions = 2000;

    // Use this for initialization
    void Start () {

        bezierPoints = new Vector3[bezierDivisions];

        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        lastTile = new GameObject();

        // pool of tiles
        tiles = new List<GameObject>();
        for (int i = 0; i < initPoolNum; ++i) {
            var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
            newTile.SetActive(false);
            tiles.Add(newTile);
        }

      
        SetTheStage();
        
    }

    /// <summary>
    /// Will spawn a tile at a certain location and setup the next position
    /// </summary>
    public GameObject GetPooledTile() {
        
        for(int i = 0; i < tiles.Count; i++) {
            if (!tiles[i].activeInHierarchy) {

                tiles[i].GetComponent<TileBehaviour>().movementSpeed = tileMovementSpeed;
                return tiles[i];
            }
        }

        return null;
    }

    //Initialize the stage
    void SetTheStage() {

        settingTheStage = true;

        for (int i = 0; i < initSpawnNum; i++) {
            SpawnNextTile();
        }

        Vector3 endPoint = nextTilePos;
        endPoint.x += 1000;
        MakeBezierCurve(nextTilePos, endPoint, Vector3.zero, Vector3.zero, bezierDivisions);

        settingTheStage = false;
    }

    Vector3 nextTilePos;
    int bezierIterator = 0;

    public void SpawnNextTile() {

       
        var newTile = GetPooledTile();        

        // Figure out where and at what rotation we should spawn
        // the next item
        if (settingTheStage) {
            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;

            nextTilePos = newTile.transform.position;
            nextTilePos.x += spaceBetweenTiles;
            nextTileLocation = nextTilePos;

            lastTile = newTile;

            nextTilePos = lastTile.transform.position;
        }
        else {
                     

            /*nextTilePos.x += spaceBetweenTiles;
            nextTilePos.y = actualNumber;*/

            nextTileLocation = bezierPoints[bezierIterator];
            bezierIterator++;

            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;
        }

        newTile.SetActive(true);
       
    }

    float GetRandomNum(float minValue, float maxValue) {

        float result;
        Random.State originalRandomState = Random.state;

        var seed = Random.Range(0, int.MaxValue);
        seed ^= (int)System.DateTime.Now.Ticks;
        seed ^= (int)Time.unscaledTime;
        seed &= int.MaxValue;
        
        Random.InitState(seed);

        result = Random.Range(minValue, maxValue);

        Random.state = originalRandomState;

        return result;
    }

    void MakeBezierCurve(Vector3 startPoint, Vector3 endPoint, 
        Vector3 startTangent, Vector3 endTangent, int division) {

       bezierPoints = Handles.MakeBezierPoints(
           startPoint, 
           endPoint, 
           startTangent, 
           endTangent,
           division
           );



    }

}

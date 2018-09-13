using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameController : MonoBehaviour {

    [Tooltip("A reference to the tile we want to spawn")]
    public GameObject tile;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(-5, -9, -3);

    [Tooltip("How many tiles should we create for the pool")]
    [Range(1, 100)]
    public int initPoolNum = 40;

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 100)]
    public int initSpawnNum = 10;

    [Tooltip("Space between the last tile and the new one")]
    [Range(1, 2)]
    public float spaceBetweenTiles = 1.1f;

    [Tooltip("Space between the top tile and the bottom one")]
    [Range(20, 50)]
    public float spaceBetweenTilesY = 20f;

    [Tooltip("The speed of the tiles")]
    [Range(-1, 0)]
    public float tileMovementSpeed = -0.16f;

    [Header("Bezier Curve")]
    [Tooltip("The start point of the bezier curve")]
    public Vector3 startBezierPoint;

    [Tooltip("The end point of the bezier curve")]
    public Vector3 endBezierPoint;

    [Tooltip("The start tangent of the bezier curve")]
    public Vector3 startTangent;

    [Tooltip("The end tangent of the bezier curve")]
    public Vector3 endTangent;

    [Tooltip("The number of divions of the bezier curve")]
    public int Divisions;
    

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

    //bezier curve
    private Vector3[] bezierPoints;
    private int bezierIterator;

    // iterator to change material colors in the tiles
    private float colorIterator;


    void Start () {

        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        colorIterator = 0;

        bezierIterator = 0;
        //create the bezier points
        bezierPoints = Handles.MakeBezierPoints(
            startBezierPoint, 
            endBezierPoint, 
            startTangent, 
            endTangent, 
            10
            );

        lastTile = new GameObject();

        // pool of tiles
        tiles = new List<GameObject>();
        for (int i = 0; i < initPoolNum; ++i) {
            var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
            newTile.GetComponent<TileBehaviour>().movementSpeed = tileMovementSpeed;
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

                
                return tiles[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Initialize the stage
    /// </summary>
    void SetTheStage() {

        settingTheStage = true;

        for (int i = 0; i < initSpawnNum; i++) {
            SpawnNextTile();
        }

        settingTheStage = false;
    }

   
    /// <summary>
    /// Spawn the next tile in the correct position
    /// </summary>
    public void SpawnNextTile() {

        Vector3 nextTilePos;
        var newTile = GetPooledTile();        

        // Figure out where and at what rotation we should spawn
        // the next item
        if (settingTheStage) {

            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;

            nextTilePos = newTile.transform.position;
            nextTilePos.x += spaceBetweenTiles;
            nextTileLocation = nextTilePos;
        }
        else {
            nextTilePos = lastTile.transform.position;
            nextTilePos.x += spaceBetweenTiles;
            nextTilePos.y = BezierPoint(ref bezierIterator).y;
            nextTileLocation = nextTilePos;

            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;

            //active separate effect
            newTile.GetComponent<TileBehaviour>().separateEffectEnable = true;
        }

        newTile.SetActive(true);
        lastTile = newTile;
    }

    /// <summary>
    /// Retuns a point from the the bezier list
    /// </summary>
    /// <param name="iterator">the position in the bezier array</param>
    /// <returns> a point in the bezier curve</returns>
    Vector3 BezierPoint(ref int iterator) {

        Vector3 point = bezierPoints[iterator];

        if (bezierIterator < bezierPoints.Length - 1) {
            iterator++;
        }
        else {
            iterator = 0;
        }

        return point;
    }

    public float GetColorIterator() {
        if(colorIterator < 1.0f) {
            colorIterator += 0.1f;
        }
        else {
            colorIterator -= 0.1f;
        }

        float result = colorIterator;
                   
        return result;
    }

}

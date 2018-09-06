using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    bool settingTheStage;


    // Use this for initialization
    void Start () {

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

        settingTheStage = false;
    }

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
            nextTileLocation = nextTilePos;

            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;
        }

        newTile.SetActive(true);
        lastTile = newTile;
    }

}

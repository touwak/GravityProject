using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Tooltip("A reference to the tile we want to spawn")]
    public GameObject tile;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(-5, -9, -3);

    [Tooltip("How many tiles should we create for the pool")]
    [Range(1, 30)]
    public int initPoolNum = 30;

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 30)]
    public int initSpawnNum = 10;

    /// <summary>
    /// Where the next tile should be spawned at.
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// How should the next tile be rotated?
    /// </summary>
    private Quaternion nextTileRotation;

    private List<GameObject> tiles;


    // Use this for initialization
    void Start () {

        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

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

    void SetTheStage() {

        for (int i = 0; i < initSpawnNum; i++) {
            var newTile = GetPooledTile();
            newTile.transform.position = nextTileLocation;
            newTile.transform.rotation = nextTileRotation;

            newTile.SetActive(true);

            // Figure out where and at what rotation we should spawn
            // the next item
            var nextTile = newTile.transform.position;
            nextTile.x += 1.1f;
            nextTileLocation = nextTile;
            nextTileRotation = Quaternion.identity;

        }
    }
}

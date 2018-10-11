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
    public float spaceBetweenCols = 1.1f;

    [Tooltip("Space between the top tile and the bottom one")]
    [Range(20, 50)]
    public float spaceBetweenTilesY = 20f;

    [Tooltip("The speed of the tiles")]
    [Range(-0.1f, -10f)]
    public float tileMovementSpeed = -0.16f;

    [Header("Y Positions")]
    [Tooltip("The start point of the Y positions")]
    [Range(-5, 5)]
    public float startYPoint;

    [Tooltip("The end point of the Y positions")]
    [Range(0, 20)]
    public float endYPoint;

    [Tooltip("The offset of the curve")]
    [Range(0.01f, 4)]
    public float offset;
    

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

    //Curve
    private List<float> yPoints;
    private List<float> yPoints2;
    private int yPointIterator;
    bool goForward;
    bool changeDifficult;

    //color
    Color[] colors;
    Color secondColor;

    void Start () {

        colors = new Color[] {
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.white,
            Color.yellow
        };

        SetSecondColor();

        goForward = true;
        changeDifficult = false;

        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        // Y points
        yPointIterator = 0;
        yPoints = new List<float>();
        SetYPoints(startYPoint, endYPoint, offset);

        lastTile = new GameObject();

        // pool of tiles
        tiles = new List<GameObject>();
        for (int i = 0; i < initPoolNum; ++i) {
            var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
            TileBehaviour tileBehaviour = newTile.GetComponent<TileBehaviour>();
            tileBehaviour.SetMovementSpeed(tileMovementSpeed);
            tileBehaviour.SetSpaceBetweenTilesY(spaceBetweenTilesY);
            tileBehaviour.SetSecondColor(secondColor);
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
            nextTilePos.x += spaceBetweenCols;
            nextTileLocation = nextTilePos;
        }
        else {
            nextTilePos = lastTile.transform.position;
            nextTilePos.x += spaceBetweenCols;
            nextTilePos.y = GetYPoint(ref yPointIterator);
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
    /// The different points of the curve
    /// </summary>
    /// <param name="startValue">first point of the curve</param>
    /// <param name="endValue">last point of the curve</param>
    /// <param name="offset">offset between points</param>
    void SetYPoints(float startValue, float endValue, float offset) {

        List<float> result = new List<float>();

        float currentValue = startValue;

        while(currentValue < endValue) {
            result.Add(currentValue);

            currentValue += offset;
        } 

        if(yPoints.Count == 0) {
            yPoints = result;
        }
        else {
            yPoints2 = result;
            changeDifficult = true;
        }
    }   

    /// <summary>
    /// Retuns a point from the the list
    /// </summary>
    /// <param name="iterator">the position in the point array</param>
    /// <returns> a point in the curve</returns>
    float GetYPoint(ref int iterator) {

        if(iterator > yPoints.Count) {          
            iterator = yPoints.Count - 1;
        }
        else if(iterator < 0) {
            iterator = 0;
        }

       float point = yPoints[iterator];

        if (goForward) {
            if (yPointIterator < yPoints.Count - 1) {
                iterator++;
            }
            else {
                goForward = false;

                if (changeDifficult) {
                    ChangeDifficult(goForward, ref iterator);
                }
            }
        }
        else {
            if (yPointIterator > 0) {
                iterator--;
            }
            else {
                goForward = true;

                if (changeDifficult) {
                    ChangeDifficult(goForward, ref iterator);
                }
            }
        }

        return point;
    }

    #region DIFFICULT

    /// <summary>
    /// Set the new Y points, the tiles speed and the separation between
    /// </summary>
    public void SetDifficult() {
        for (int i = 0; i < tiles.Count; i++) {
            tiles[i].GetComponent<TileBehaviour>().SetMovementSpeed(tileMovementSpeed);
            tiles[i].GetComponent<TileBehaviour>().SetSpaceBetweenTilesY(spaceBetweenTilesY);
        }

        SetYPoints(startYPoint, endYPoint, offset);
    }

    /// <summary>
    /// Change the difficult setting the new Y points and set the iterator
    /// </summary>
    /// <param name="forward">the direction of the reading</param>
    /// <param name="iterator">the iterator of the array</param>
    void ChangeDifficult(bool forward, ref int iterator) {
        yPoints.Clear();

        for(int i = 0; i < yPoints2.Count; i++) {
            yPoints.Add(yPoints2[i]);           
        }

        yPoints2.Clear();

        if (!forward) {
            iterator = yPoints.Count - 1;
        }
        else {
            iterator = 0;
        }

        changeDifficult = false;

        SetTileSecondColor();
    }

    #endregion

    Color RandColor() {
        int i = Random.Range(0, colors.Length);

        return colors[i];
    }

    void SetSecondColor() {
        secondColor = RandColor();
    }

    void SetTileSecondColor() {
        SetSecondColor();

        for(int i = 0; i < tiles.Count; i++) {
            tiles[i].GetComponent<TileBehaviour>().SetSecondColor(secondColor);
        }
    }
}

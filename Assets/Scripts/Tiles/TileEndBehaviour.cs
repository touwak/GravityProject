using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehaviour : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Tile")) {

            other.gameObject.SetActive(false);
            FindObjectOfType<GameController>().SpawnNextTile();

        }
    }
}

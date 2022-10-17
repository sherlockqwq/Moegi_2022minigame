using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public GameObject tile ;
    public bool canGet = false ; 

    public bool getState(){
        return canGet ; 
    }

    public GameObject getTile(){
        return gameObject ; 
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
            if(other.CompareTag("Tile") || other.CompareTag("collection")|| other.CompareTag("X_Tile"))
        {
                tile = other.gameObject;
                canGet = true ;
        }
        
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tile") || other.CompareTag("collection") || other.CompareTag("X_Tile"))
        {
            tile = null;
            canGet = false;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public GameObject tile ;
    public bool canGet = false ; 

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(tile != null){
            if(other.CompareTag("tile")){
                tile = other.gameObject;
                canGet = false ; 
            }
        }
    }
}

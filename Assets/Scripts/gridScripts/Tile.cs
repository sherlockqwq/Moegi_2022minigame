using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
     private Color baseColor, offsetColor;
     private SpriteRenderer TileRenderer;
    private BoxCollider2D coll;
    private void Awake()
    {
        TileRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

    }
    public void Init(bool isoffset)
    {

        /*TileRenderer.color = isoffset ? offsetColor : baseColor;
        */
    }

}

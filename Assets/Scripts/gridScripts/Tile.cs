using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer TileRenderer;
    private void Awake()
    {
        TileRenderer = GetComponent<SpriteRenderer>();
    }
    public void Init(bool isoffset)
    {

        TileRenderer.color = isoffset ? offsetColor : baseColor;
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    private void Start()
    {
        generateGrid();
    }
    void generateGrid()

    {
        GameObject TileController = new GameObject("TileController");
        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity,TileController.transform);
                spawnedTile.name = $"Tile{x} {y}";
                bool isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);
                
            }
        }
    }
}

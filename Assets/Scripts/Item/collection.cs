using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collection : MonoBehaviour
{
    private void Start()
    {
        TileManager.Instance.RegisterCollections(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TileManager.Instance.removeCollection(this);
            Destroy(gameObject);
        }
    }
}

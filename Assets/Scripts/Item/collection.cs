using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collection : MonoBehaviour
{
    public AudioClip sfx ; 
    private void Start()
    {
        TileManager.Instance.RegisterCollections(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TileManager.Instance.removeCollection(this);
            GameAudio.PlaySFX(sfx);
            Destroy(gameObject);
        }
    }
}

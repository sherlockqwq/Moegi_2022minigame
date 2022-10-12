using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Tile : MonoBehaviour
{
    private Color baseColor, offsetColor;
    private SpriteRenderer TileRenderer;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private GameObject overlapTile;
    public bool isdragging = false;
    SpriteRenderer targetRenderer;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;


    private void Awake()
    {
        TileRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        rb.isKinematic = true;
        coll.size = new Vector2(0.9f, 0.9f);
    }
    public void Init(bool isoffset)
    {

        /*TileRenderer.color = isoffset ? offsetColor : baseColor;
        */
    }

    public void FadeIn()
    {

    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutZoom());
    }
    IEnumerator FadeOutZoom()
    {
        float time = 0f;
        while (time < fadeOutTime)
        {
            transform.localScale = transform.localScale * Mathf.Lerp(1, 0, time / fadeOutTime);

            time += Time.deltaTime;
        }
        yield break;
    }
    /*public void DetectTileOverlap()
    {


        var overlapTile = Physics2D.OverlapCircle(transform.position, 0.01f);
        if (overlapTile != null && overlapTile.CompareTag("Tile") && overlapTile.gameObject.layer == LayerMask.NameToLayer("TileLayer"))
        {
            Debug.Log(name + overlapTile.name);
            targetRenderer = overlapTile.gameObject.GetComponent<SpriteRenderer>();
            targetRenderer.color = Color.red;
        }
        else
        {
            targetRenderer.color = Color.green;
        }

    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile") && isdragging)
        {
            Debug.Log(name + "½øÈë" + collision.name + "µÄtrigger");
            collision.GetComponent<SpriteRenderer>().color = Color.red;
            overlapTile = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile") && isdragging)
        {
            collision.GetComponent<SpriteRenderer>().color = Color.white;
            overlapTile = null;
        }
    }
    public void SpriteTranslucent()
    {
        TileRenderer.color = new Color(1, 1, 1, 0.5f);
    }
    public void SpriteToNormal()
    {
        TileRenderer.color = Color.white;
    }
    public GameObject getOverlapTile()
    {
        return overlapTile;
    }

}

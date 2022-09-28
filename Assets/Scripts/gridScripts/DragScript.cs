using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DragScript : MonoBehaviour
{
    [SerializeField] public bool isDraging=false;
    [SerializeField] public bool isDragable=true;
    [SerializeField] private bool snapToGrid = true;
    [SerializeField] private float gridSize=1f;
    [SerializeField] private GameObject dragObject;


    private BoxCollider2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        rb.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        /*if (!isDragable)
            return;
        GameObject dragObject = Instantiate(gameObject);
        dragObject.GetComponent<DragScript>().isDraging = true;
        dragObject.GetComponent<DragScript>().isDragable = true;*/


    }
    private void OnMouseDrag()
    {
        if (!isDragable)
            return;
        Cursor.visible = false;
        
        transform.position =(Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (gridSize > 0&&snapToGrid)
        {
            transform.position = new Vector2(Mathf.RoundToInt(transform.position.x/gridSize)*gridSize, Mathf.RoundToInt(transform.position.y/gridSize)*gridSize);
        }
    }
    
    private void OnMouseUp()
    {
        Cursor.visible = true;
        isDragable = false;
    }
}

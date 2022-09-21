using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    [SerializeField] private bool isDraging=false;
    [SerializeField] private bool isDragable=true;
    [SerializeField] private bool snapToGrid = true;
    [SerializeField] private float gridSize=1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}

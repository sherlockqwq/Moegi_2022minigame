using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DragScript : MonoBehaviour
{
    
    [SerializeField] public bool isDragable=true;
    [SerializeField] private bool snapToGrid = true;
    [SerializeField] private float gridSize=1f;



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
        if (TileManager.Instance.dragNumInScene <=0|| !isDragable)
            return;
        /*Cursor.visible = false;*/
        
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (gridSize > 0&&snapToGrid)
        {
            transform.position = new Vector2(Mathf.RoundToInt(transform.position.x/gridSize)*gridSize, Mathf.RoundToInt(transform.position.y/gridSize)*gridSize);
        }
        if (Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 90));
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                child.transform.rotation = Quaternion.Euler(child.transform.rotation.eulerAngles + new Vector3(0, 0, -90));

            }
            /*Transform[] childrenTransform = GetComponentsInChildren<Transform>();

            foreach (var child in childrenTransform)
            {
                child.rotation = Quaternion.Euler(child.rotation.eulerAngles + new Vector3(0, 0, -90));
            }*/

        }
    }
    
    private void OnMouseUp()
    {
        Cursor.visible = true;
        isDragable = false;
        rb.enabled = false;
        TileManager.Instance.dragNumInScene -= 1;
    }
}

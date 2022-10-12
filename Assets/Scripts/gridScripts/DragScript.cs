using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DragScript : MonoBehaviour
{

    [SerializeField] public bool isDragable = true;//能否被拖拽，被拖拽后自动变为不可拖拽状态
    [SerializeField] private bool snapToGrid = true;//是否启动吸附
    [SerializeField] private float gridSize = 1f;//格子吸附时的格子大小
    [SerializeField] private Vector3 originalPosition;//拖拽前原本的位置



    private BoxCollider2D coll;
    // Start is called before the first frame update
    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();

    }
    void Start()
    {
        coll.isTrigger = true;
        TileManager.Instance.RegisterModules(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.Log("点击的" +
            "名字是" +
            name);
        TileManager.Instance.CreateRegisteredModules(gameObject);



    }
    private void OnMouseDrag()
    {

        if (TileManager.Instance.dragNumInScene <= 0 || !isDragable)
            return;
        Cursor.visible = false;

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (gridSize > 0 && snapToGrid)
        {
            transform.position = new Vector2(Mathf.RoundToInt(transform.position.x / gridSize) * gridSize, Mathf.RoundToInt(transform.position.y / gridSize) * gridSize);
        }
        if (Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 90));
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                child.transform.rotation = Quaternion.Euler(child.transform.rotation.eulerAngles + new Vector3(0, 0, -90));

            }


        }


        ChildrenDragging();
    }
    void checkTilesInZone()
    {
        var gameZone = GameObject.FindGameObjectWithTag("GameZone");
        var gameZoneColl = GetComponent<BoxCollider2D>();

        if (gameZoneColl.bounds.Contains(transform.position))
        {
            Debug.Log(" 已经在可放置区域");
        }
        else
        {
            Debug.Log("不在可放置区域");
        }
    }
    private void OnMouseUp()
    {
        ChildrenDragFinished();

        Cursor.visible = true;
        isDragable = false;
        coll.enabled = false;
        TileManager.Instance.dragNumInScene -= 1;

    }
    /*private void checkTilesOverlap()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<Tile>();
            child.DetectTileOverlap();
        }
    }*/
    private void ChildrenDragging()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<Tile>();
            child.SpriteTranslucent();
            child.isdragging = true;
        }
    }
    private void ChildrenDragFinished()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<Tile>();
            child.SpriteToNormal();
            child.isdragging = false;
            if (child.getOverlapTile() != null)
            {
                Destroy(child.getOverlapTile());
                Destroy(child.gameObject);
            }
        }
    }






}

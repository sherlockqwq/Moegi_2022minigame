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
    private PlayerControl player;

    public AudioClip clips; 

    private BoxCollider2D coll;
    // Start is called before the first frame update
    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        clips = Resources.Load<AudioClip>("3-重叠脱落音");

    }
    void Start()
    {
        coll.isTrigger = true;
        TileManager.Instance.RegisterModules(gameObject);
        setChildrenCollider(false);

    }

    // Update is called once per frame

    private void OnMouseDown()
    {
        setChildrenCollider(true);

        TileManager.Instance.CreateRegisteredModules(gameObject);
        isDragable = true;
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
        if (Input.GetMouseButtonDown(1))//右键旋转
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
        if (TileManager.Instance.CheckForPlacement(gameObject))
        {
            TileManager.Instance.dragNumInScene -= 1;
        }
        else
        {
            Destroy(gameObject);
        }

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
                if (child.getOverlapTile().CompareTag("Tile"))
                {

                    Destroy(child.getOverlapTile());
                    Destroy(child.gameObject);
                    GameAudio.PlaySFX(clips);
                }
                else if (child.getOverlapTile().CompareTag("X_Tile"))
                {
                    GameAudio.PlaySFX(clips);
                    Destroy(child.gameObject);
                }

            }
        }
        player.FreshDectors();
    }
    private void setChildrenCollider(bool stateBool)
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Tile"))
            {
                BoxCollider2D childCollider = child.GetComponent<BoxCollider2D>();
                childCollider.enabled = stateBool;
                /*Debug.Log("把" + child.name + "的碰撞体设置为" + stateBool);*/
            }
        }
    }





}

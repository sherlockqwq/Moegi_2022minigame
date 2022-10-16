using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    [Header("复制体设置")]
    public bool haveCopyPlayer ;
    public CopyPlayer theCopyPlayer; 

    [Header("Detectors")]
    [SerializeField] private float X_offset ;
    [SerializeField] private float Y_offset ; 
    [SerializeField] private TileDetector up;
    [SerializeField] private TileDetector down;
    [SerializeField] private TileDetector left;
    [SerializeField] private TileDetector right;
    private List<TileDetector> detectors = new List<TileDetector>();

    private Dictionary<TileDetector, Transform> maps = new Dictionary<TileDetector, Transform>();
    //TileDetector用于检测是否可以移动，Tranform是对应的四个方向的格子

    [SerializeField]public LayerMask groundLayer ; 
    public float groundRayLength ; 

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        detectors.Add(up);
        detectors.Add(down);
        detectors.Add(left);
        detectors.Add(right);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void linkCopyPlayer(CopyPlayer _copy)
    {
        theCopyPlayer = _copy;
        haveCopyPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveInTile())
        {
            FreshDectors();
        }
    }

    public bool MoveInTile(){
        bool result = false;
        float step = speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (down.canGet)
            {
                if (haveCopyPlayer && !theCopyPlayer.down.canGet)
                {
                    return false ;
                }
            

                transform.localPosition = down.tile.transform.position;
                theCopyPlayer.moveIt(moveDirection.down);
                //gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, down.tile.transform.localPosition, step);
                result = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (up.canGet)
            {
                if (haveCopyPlayer && !theCopyPlayer.up.canGet)
                {
                    return false;
                }

                transform.localPosition = up.tile.transform.position;
                theCopyPlayer.moveIt(moveDirection.up);

                //gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, up.tile.transform.localPosition, step);
                result = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (left.canGet)
            {
                if(haveCopyPlayer && !theCopyPlayer.left.canGet)
                {
                    return false; 
                }

                //transform.position = transform.TransformPoint(left.tile.transform.localPosition);
                transform.localPosition = left.tile.transform.position;
                theCopyPlayer.moveIt(moveDirection.left);
                //gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, left.tile.transform.localPosition, step);
                result = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (right.canGet)
            {
                if(haveCopyPlayer && !theCopyPlayer.right.canGet)
                {
                    return false;
                }

                //transform.position = transform.TransformPoint(right.tile.transform.localPosition);
                transform.localPosition = right.tile.transform.position;
                theCopyPlayer.moveIt(moveDirection.right);
                //gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, right.tile.transform.localPosition, step);
                result = true; 
            }
        }

        return result; 
    }

    private void FreshDectors()
    {
        foreach (TileDetector detector in detectors)
        {
            detector.gameObject.SetActive(false);
            Debug.Log("Update");
            detector.gameObject.SetActive(true);
        }

        up.transform.localPosition = new Vector3(up.transform.localPosition.x, Y_offset, up.transform.localPosition.z);
        down.transform.localPosition = new Vector3(down.transform.localPosition.x, -Y_offset, down.transform.localPosition.z);
        left.transform.localPosition = new Vector3(-X_offset , left.transform.localPosition.y, left.transform.localPosition.z);
        right.transform.localPosition = new Vector3(X_offset, right.transform.localPosition.y, right.transform.localPosition.z);

    }

    public void PlayerEnterExit()
    {
        if (haveCopyPlayer)
        {
            transform.position = theCopyPlayer.transform.position;
            Destroy(theCopyPlayer);
            haveCopyPlayer = false;
        }
        else
        {
            //直接过关  
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    [Header("����������")]
    public bool haveCopyPlayer;
    public CopyPlayer theCopyPlayer;

    [Header("��������")]
    public bool tileDestory;
    public AudioClip tileDestoryClips ; 

    [Header("Detectors")]
    [SerializeField] private float X_offset;
    [SerializeField] private float Y_offset;
    [SerializeField] private TileDetector up;
    [SerializeField] private TileDetector down;
    [SerializeField] private TileDetector left;
    [SerializeField] private TileDetector right;
    private List<TileDetector> detectors = new List<TileDetector>();

    public GameObject tile_Now;

    private Dictionary<TileDetector, Transform> maps = new Dictionary<TileDetector, Transform>();
    //TileDetector���ڼ���Ƿ�����ƶ���Tranform�Ƕ�Ӧ���ĸ�����ĸ���

    [SerializeField] public LayerMask groundLayer;
    public float groundRayLength;

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

    public bool MoveInTile()
    {
        bool result = false;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (down.canGet)
            {
                if (haveCopyPlayer && !theCopyPlayer.down.canGet)
                {
                    return false;
                }

                if (tileDestory)
                {
                    GameAudio.PlaySFX(tileDestoryClips);
                    Destroy(tile_Now);
                }

                transform.localPosition = down.tile.transform.position;
                if (haveCopyPlayer)
                {


                    theCopyPlayer.moveIt(moveDirection.down);
                }
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

                if (tileDestory)
                {
                        GameAudio.PlaySFX(tileDestoryClips);
                    Destroy(tile_Now);
                }

                transform.localPosition = up.tile.transform.position;
                if (haveCopyPlayer)
                {


                    theCopyPlayer.moveIt(moveDirection.up);
                }

                result = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (left.canGet)
            {
                if (haveCopyPlayer && !theCopyPlayer.left.canGet)
                {
                    return false;
                }

                if (tileDestory)
                {
                    GameAudio.PlaySFX(tileDestoryClips);
                    Destroy(tile_Now);
                }

                transform.localPosition = left.tile.transform.position;
                
                if (haveCopyPlayer)
                {

                    theCopyPlayer.moveIt(moveDirection.left);
                }
                result = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (right.canGet)
            {
                if (haveCopyPlayer && !theCopyPlayer.right.canGet)
                {
                    return false;
                }

                if (tileDestory)
                {
                    GameAudio.PlaySFX(tileDestoryClips);
                    Destroy(tile_Now);
                }

                transform.localPosition = right.tile.transform.position;

                if (haveCopyPlayer)
                {
                    theCopyPlayer.moveIt(moveDirection.right);
                }
                result = true;
            }
        }

        return result;
    }

    public void FreshDectors()
    {
        foreach (TileDetector detector in detectors)
        {
            detector.gameObject.SetActive(false);
            detector.gameObject.SetActive(true);
        }

        up.transform.localPosition = new Vector3(up.transform.localPosition.x, Y_offset, up.transform.localPosition.z);
        down.transform.localPosition = new Vector3(down.transform.localPosition.x, -Y_offset, down.transform.localPosition.z);
        left.transform.localPosition = new Vector3(-X_offset, left.transform.localPosition.y, left.transform.localPosition.z);
        right.transform.localPosition = new Vector3(X_offset, right.transform.localPosition.y, right.transform.localPosition.z);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tile"))
        {
            tile_Now = other.gameObject;
        }

    }


    public void PlayerEnterExit(string _EnterName)
    {
        Debug.Log(_EnterName);
        if (haveCopyPlayer)
        {
            if(_EnterName == "Player")
            {
                Debug.Log("Player����");
                transform.position = theCopyPlayer.transform.position;
                Destroy(theCopyPlayer.gameObject);
                gameObject.name = "CopyPlayer(Clone)";
                haveCopyPlayer = false;
            }
            else
            {
                Debug.Log("�������Ƚ���");
                Destroy(theCopyPlayer.gameObject);
                haveCopyPlayer = false;
            }
        }
        

    }

    public bool getHaveCopy()
    {
        return haveCopyPlayer;
    }

}

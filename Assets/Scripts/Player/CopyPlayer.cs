using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum moveDirection { down,up,left,right};
public class CopyPlayer : MonoBehaviour
{
    [Header("Detectors")]
    [SerializeField] private float X_offset;
    [SerializeField] private float Y_offset;
    public TileDetector up;
    public TileDetector down;
    public TileDetector left;
    public TileDetector right;
    private List<TileDetector> detectors = new List<TileDetector>();

    private Dictionary<TileDetector, Transform> maps = new Dictionary<TileDetector, Transform>();

    private void Awake()
    {
        detectors.Add(up);
        detectors.Add(down);
        detectors.Add(left);
        detectors.Add(right);
    }

    private void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerControl>().linkCopyPlayer(this);
        Debug.Log("Íê³É");
    }
    public bool moveIt(moveDirection direction)
    {
        bool result = false;

        if(direction == moveDirection.up)
        {
            transform.localPosition = up.tile.transform.position;
            FreshDectors();

        }

        if (direction == moveDirection.down)
        {
            transform.localPosition = down.tile.transform.position;
            FreshDectors();
        }

        if (direction == moveDirection.left)
        {
            transform.localPosition = left.tile.transform.position;
            FreshDectors();

        }

        if (direction == moveDirection.right)
        {
            transform.localPosition = right.tile.transform.position;
            FreshDectors();
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
        left.transform.localPosition = new Vector3(-X_offset, left.transform.localPosition.y, left.transform.localPosition.z);
        right.transform.localPosition = new Vector3(X_offset, right.transform.localPosition.y, right.transform.localPosition.z);

    }

}

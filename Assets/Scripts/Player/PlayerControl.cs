using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    [SerializeField]public LayerMask groundLayer ; 
    public float groundRayLength ; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckGround(){
        //更新四周是否有可以移动的地块
            RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,groundRayLength,groundLayer); //单一射线检测
            down = hit.collider.gameObject  ; 
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red ; 
    //     Gizmos.DrawLine(transform.position , transform.position + Vector3.down * groundRayLength);
    //     Gizmos.DrawLine(transform.position , transform.position + Vector3.up * groundRayLength);
    //     Gizmos.DrawLine(transform.position , transform.position + Vector3.left * groundRayLength);
    //     Gizmos.DrawLine(transform.position , transform.position + Vector3.right * groundRayLength);
    // }
}

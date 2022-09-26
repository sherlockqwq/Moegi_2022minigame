using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            GetPoint();
        }
    }

    private void GetPoint()
    {
        // 碰到收集品之后触发什么写在这里
        Debug.Log("得到收藏品");
        Destroy(gameObject);
    }
}

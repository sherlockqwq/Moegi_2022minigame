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
        // �����ռ�Ʒ֮�󴥷�ʲôд������
        Debug.Log("�õ��ղ�Ʒ");
        Destroy(gameObject);
    }
}

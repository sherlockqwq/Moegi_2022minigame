using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum theEvent {  collection, deadArea, exit ,copy };

public class eventTile : MonoBehaviour
{
    public int exitCondition = 1; //Ҫ���ν����յ����
    [SerializeField] private int exitCount;

    public theEvent choseEvent;

    public GameObject theCopyPlayer;
    public Transform copyPlayerLocation;
    [SerializeField] private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            if(choseEvent == theEvent.collection)
            {
                GetPoint();
            }

            if (choseEvent == theEvent.deadArea)
            {
                deadArea();
            }

            if (choseEvent == theEvent.exit)
            {
                exitScene();
            }

            if(choseEvent == theEvent.copy)
            {
                if (!isUsed)
                {
                    copyIt();
                    isUsed = true;
                }
            }
        }
    }

    private void GetPoint()
    {
        // �����ռ�Ʒ֮�󴥷�ʲôд������
        Debug.Log("�õ��ղ�Ʒ");
        Destroy(gameObject);
    }

    private void deadArea()
    {
        Debug.Log("X area");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void exitScene()
    {
        GameObject.Find("Player").GetComponent<PlayerControl>().PlayerEnterExit();
        exitCount++;
        if(exitCount >= exitCondition)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void copyIt() { 
        Vector3 _position = new Vector3(copyPlayerLocation.position.x, copyPlayerLocation.position.y, copyPlayerLocation.position.z);
        GameObject.Instantiate(theCopyPlayer,_position,Quaternion.identity);
    }
}

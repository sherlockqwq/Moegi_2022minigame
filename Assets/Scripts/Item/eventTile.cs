using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum theEvent {  collection, deadArea, exit };

public class eventTile : MonoBehaviour
{
    public theEvent choseEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            GetPoint();
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
        }
    }

    private void GetPoint()
    {
        // 碰到收集品之后触发什么写在这里
        Debug.Log("得到收藏品");
        Destroy(gameObject);
    }

    private void deadArea()
    {
        Debug.Log("X area");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void exitScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

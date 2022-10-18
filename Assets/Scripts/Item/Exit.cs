using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Exit : MonoBehaviour
{
    public string sceneName;
    private SpriteRenderer exitRenderer;

    [Header("4-6关复制体部分专用")]
    [SerializeField] private bool needCopy; 
    [SerializeField] private bool theCopyEnter = true; // 需要两个都进入
    [SerializeField] private bool PlayerEnter = false; // 需要两个都进入 
    [SerializeField] private PlayerControl player; 

    private void Awake()
    {
        exitRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("Player") && TileManager.Instance.getCollectionsCount() <= 0)
        {

            if (needCopy)
            {
                if(collision.name == "CopyPlayer(Clone)")
                {
                    theCopyEnter = true;
                    player.PlayerEnterExit(collision.name);
                }

                if (collision.name == "CopyPlayer(Clone)" && PlayerEnter)
                {
                    GameManager.Instance.LoadSceneByName(sceneName);
                }

                if (collision.name == "Player" && !theCopyEnter && player.getHaveCopy())
                {
                    PlayerEnter = true; 
                    player.PlayerEnterExit(collision.name);
                }

                if (collision.name == "Player" && theCopyEnter)
                {
                    GameManager.Instance.LoadSceneByName(sceneName);
                }


            }
            else
            {
                GameManager.Instance.LoadSceneByName(sceneName);
            }

        }
    }
    private void Update()
    {
        switchColor();
    }
    void switchColor()
    {
        if (TileManager.Instance.getCollectionsCount() > 0)
        {
            exitRenderer.color = new Color(0.5f, .5f, .5f, 1);
        }
        else if (TileManager.Instance.getCollectionsCount() <= 0)
        {
            exitRenderer.color = Color.white;
        }
    }
}

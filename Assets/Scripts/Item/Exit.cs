using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Exit : MonoBehaviour
{
    public string sceneName;
    private SpriteRenderer exitRenderer;

    [Header("4-6�ظ����岿��ר��")]
    [SerializeField] private bool needDouble; // ��Ҫ����������    

    private void Awake()
    {
        exitRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && TileManager.Instance.getCollectionsCount() <= 0)
        {
            if (!needDouble)
            {
                GameManager.Instance.LoadSceneByName(sceneName);
            }
            else
            {
                needDouble = false;
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

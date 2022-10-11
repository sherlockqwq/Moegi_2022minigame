using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TileManager : Singleton<TileManager>
{

    public int dragNumInScene;
    [SerializeField] private DragNumber_SO dragNumberData;

    [SerializeField] private Text UI_DragNumber; 
    private void Start()
    {

        if (dragNumberData.sceneDragNumber.ContainsKey(SceneManager.GetActiveScene().name))
        {
            string dragNumInData= dragNumberData.sceneDragNumber[SceneManager.GetActiveScene().name];

            Debug.Log(dragNumInData);
            dragNumInScene = int.Parse(dragNumInData);
        }
        else
        {
            Debug.Log("������������scenename����");
            dragNumInScene = -1;
        }

        UI_DragNumber = GameObject.Find("UI").transform.Find("DragNumberUI").gameObject.GetComponent<Text>();
        
    }

    private void Update()
    {
        //����UI�ĸ�����������д��Update����������
        //�б�Ҫ�Ļ������ǵø�

        UI_DragNumber.text = (dragNumInScene).ToString();
        //������Ϊ��ק�ж����� > 0 ,����ʵ����Ŀ��-1
    }
}

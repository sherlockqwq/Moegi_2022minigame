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
            Debug.Log("不存在这样的scenename数据");
            dragNumInScene = -1;
        }

        UI_DragNumber = GameObject.Find("UI").transform.Find("DragNumberUI").gameObject.GetComponent<Text>();
        
    }

    private void Update()
    {
        //对于UI的更新我这里先写在Update函数里面了
        //有必要的话后续记得改

        UI_DragNumber.text = (dragNumInScene).ToString();
        //这里因为拖拽判定的是 > 0 ,所以实际数目需-1
    }
}

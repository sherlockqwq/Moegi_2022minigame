using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : Singleton<TileManager>
{

    public int dragNumInScene;
    [SerializeField] private DragNumber_SO dragNumberData;
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
        
    }
}

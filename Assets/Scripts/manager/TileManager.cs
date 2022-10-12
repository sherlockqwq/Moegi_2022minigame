using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : Singleton<TileManager>
{

    public int dragNumInScene;
    [SerializeField] private DragNumber_SO dragNumberData;
    [SerializeField] private List<GameObject> dragModules;
    [SerializeField] private List<Transform> modulesTransform;
    private void Start()
    {

        if (dragNumberData.sceneDragNumber.ContainsKey(SceneManager.GetActiveScene().name))
        {
            string dragNumInData = dragNumberData.sceneDragNumber[SceneManager.GetActiveScene().name];

            Debug.Log(dragNumInData);
            dragNumInScene = int.Parse(dragNumInData);
        }
        else
        {
            Debug.Log("不存在这样的scenename数据");
            dragNumInScene = -1;
        }

    }
    public void RegisterModules(GameObject modules)//注册场景里符合要求的模块
    {
        if (!dragModules.Contains(modules))
        {
            dragModules.Add(modules);
            modulesTransform.Add(modules.transform);
        }
    }
    public void CreateRegisteredModules(GameObject modules)
    {
        if (dragModules.Contains(modules) && dragNumInScene > 0)
        {
            int gameobjectIndex = dragModules.IndexOf(modules);
            Instantiate(dragModules[gameobjectIndex], modulesTransform[gameobjectIndex].position, modulesTransform[gameobjectIndex].rotation);
        }
    }
}

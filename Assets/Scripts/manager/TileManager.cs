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

    [SerializeField] private List<GameObject> dragModules;
    [SerializeField] private List<Transform> modulesTransform;
    [SerializeField] private List<collection> collections;
    private LineRenderer lr;
    Vector3 lowerleftPoint;

    Vector3 toprightPoint;
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

        UI_DragNumber = GameObject.Find("UI").transform.Find("DragNumberUI").gameObject.GetComponent<Text>();
        lr = GetComponent<LineRenderer>();
        lowerleftPoint = lr.GetPosition(0);
        toprightPoint = lr.GetPosition(2);
        lr.enabled = false;
    }
    private void Update()
    {
        //对于UI的更新我这里先写在Update函数里面了
        //有必要的话后续记得改

        UI_DragNumber.text = (dragNumInScene).ToString();
    }
    public void RegisterModules(GameObject modules)//注册场景里符合要求的模块
    {
        if (!dragModules.Contains(modules))
        {
            dragModules.Add(modules);
            modulesTransform.Add(modules.transform);
        }
    }
    public void RegisterCollections(collection collection)//注册场景里符合要求的收集物
    {
        if (!collections.Contains(collection))
        {
            collections.Add(collection);
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
    public void removeCollection(collection collection)
    {
        if (collections.Contains(collection))
        {
            collections.Remove(collection);
        }
    }
    public int getCollectionsCount()
    {
        return collections.Count;
    }
    public bool CheckForPlacement(GameObject placedModule)
    {

        Debug.Log("左下点" + lowerleftPoint);
        Debug.Log("右上点" + toprightPoint);
        if (!dragModules.Contains(placedModule))
        {
            Debug.Log("这个模块没有注册到TileManager里");
            return false;
        }
        else if (RectCheck(placedModule.transform.position))
        {

            return true;
        }
        else
        {
            return false;
        }
    }
    private bool RectCheck(Vector3 point)
    {
        if (point.x < lowerleftPoint.x || point.x > toprightPoint.x || point.y < lowerleftPoint.y || point.y > toprightPoint.y)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}

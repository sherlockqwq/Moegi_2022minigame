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
            Debug.Log("������������scenename����");
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
        //����UI�ĸ�����������д��Update����������
        //�б�Ҫ�Ļ������ǵø�

        UI_DragNumber.text = (dragNumInScene).ToString();
    }
    public void RegisterModules(GameObject modules)//ע�᳡�������Ҫ���ģ��
    {
        if (!dragModules.Contains(modules))
        {
            dragModules.Add(modules);
            modulesTransform.Add(modules.transform);
        }
    }
    public void RegisterCollections(collection collection)//ע�᳡�������Ҫ����ռ���
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

        Debug.Log("���µ�" + lowerleftPoint);
        Debug.Log("���ϵ�" + toprightPoint);
        if (!dragModules.Contains(placedModule))
        {
            Debug.Log("���ģ��û��ע�ᵽTileManager��");
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

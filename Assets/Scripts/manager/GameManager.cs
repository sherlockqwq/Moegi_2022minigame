using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{

    
    override protected void  Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        ReLoadScene();  
    }
    void ReLoadScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}

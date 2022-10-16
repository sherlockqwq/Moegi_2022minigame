using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{


    override protected void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReLoadScene();
        }
    }
    void ReLoadScene()
    {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

    }
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        yield return 1;
    }
}

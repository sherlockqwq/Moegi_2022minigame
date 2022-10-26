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

    public void callReLoadScene()
    {
        ReLoadScene();
    }
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));

    }

    IEnumerator LoadScene(string sceneName) {
		yield return StoryScene.TransitionManager.Current.ShowMaskCoroutine();

		yield return SceneManager.LoadSceneAsync(sceneName);

		yield return StoryScene.TransitionManager.Current.HideMaskCoroutine();
    }
}

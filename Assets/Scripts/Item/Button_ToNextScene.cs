using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_ToNextScene : MonoBehaviour
{
    public string nextScene ;  

    public void toNextScene(string _sceneName){
        SceneManager.LoadScene(_sceneName);
    }
}

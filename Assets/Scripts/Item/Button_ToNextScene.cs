using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_ToNextScene : MonoBehaviour
{
    public string nextScene ;  
    public AudioClip sfx ; 

    public void toNextScene(string _sceneName){
        GameAudio.PlaySFX(sfx);
        SceneManager.LoadScene(_sceneName);
    }
}

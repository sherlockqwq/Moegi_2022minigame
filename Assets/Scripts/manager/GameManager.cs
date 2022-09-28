using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] 
    override protected void  Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

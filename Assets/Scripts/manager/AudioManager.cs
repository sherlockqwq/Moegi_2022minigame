using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Music[] MusicSounds, SFXSounds;
    [SerializeField] private AudioSource MusicSource, SFXSource;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        /*PlayMusic("BGM");*/
    }
    public void PlayMusic(string name)
    {
        Music targetMusic = null;
        foreach (var musicClip in MusicSounds)
        {
            if (musicClip.name == name)
            {
                targetMusic = musicClip;
                break;
            }
        }
        if (targetMusic == null)
        {
            Debug.Log("找不到这样的音源");
            return;
        }
        else
        {
            MusicSource.clip = targetMusic.clip;
            MusicSource.Play();
        }


    }
    public void PlaySFX(string name)
    {
        Music targetMusic = null;
        foreach (var musicClip in SFXSounds)
        {
            if (musicClip.name == name)
            {
                targetMusic = musicClip;
                break;
            }
        }
        if (targetMusic == null)
        {
            Debug.Log("找不到这样的音源");
            return;
        }
        else
        {
            SFXSource.clip = targetMusic.clip;
            SFXSource.Play();
        }


    }
}

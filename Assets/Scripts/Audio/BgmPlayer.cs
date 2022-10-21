using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour {
	[SerializeField] private AudioClip _bgm;
	void Awake() => GameAudio.FadeBGM(_bgm);
}

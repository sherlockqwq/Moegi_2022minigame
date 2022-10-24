using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour {
	[SerializeField] private AudioClip _bgm;
	[SerializeField] private float _delay;
	[SerializeField] private float _volume = 1f;
	IEnumerator Start() {
		yield return new WaitForSeconds(_delay);
		GameAudio.FadeBGM(bgm: _bgm, volume: _volume);
	}
}

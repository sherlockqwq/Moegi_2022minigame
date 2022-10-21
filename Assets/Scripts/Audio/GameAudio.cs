using System.Collections;
using System.Collections.Generic;
using EasyTools;
using UnityEngine;

public static class GameAudio {
	private static GameObject _obj;
	private static AudioSource _sfxSource, _bgmSource;
	private static bool _fading = false;

	/// <summary>
	/// 播放音效（可同时播放多个，不会覆盖）
	/// </summary>
	public static void PlaySFX(AudioClip sfx, float volumeScale = 1f) {
		if (_obj == null) Init();

		if (sfx != null) _sfxSource.PlayOneShot(sfx, volumeScale);
	}

	/// <summary>
	/// BGM淡出淡入切换
	/// </summary>
	public static void FadeBGM(AudioClip bgm, float fadeTime = 0.2f) {
		if (!_fading) EasyGameLoop.Do(BGM_Fade(bgm, fadeTime));
	}
	static IEnumerator BGM_Fade(AudioClip targetBGM, float fadeTime) {
		if (_obj == null) Init();

		if (targetBGM != _bgmSource.clip) {
			_fading = true;

			var v = _bgmSource.volume;
			yield return EasyTools.Gradient.Linear(fadeTime, d => _bgmSource.volume = Mathf.Lerp(v, 0, d));
			_bgmSource.Stop();
			_bgmSource.clip = targetBGM;
			if (targetBGM != null) {
				_bgmSource.Play();
				yield return EasyTools.Gradient.Linear(fadeTime, d => _bgmSource.volume = Mathf.Lerp(0, v, d));
			}
			else {
				_bgmSource.volume = v;
			}

			_fading = false;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init() {
		_obj = new GameObject("GameAudio");
		_sfxSource = _obj.AddComponent<AudioSource>();
		_bgmSource = _obj.AddComponent<AudioSource>();
		_bgmSource.playOnAwake = false;
		_bgmSource.loop = true;
		Object.DontDestroyOnLoad(_obj);
	}
}

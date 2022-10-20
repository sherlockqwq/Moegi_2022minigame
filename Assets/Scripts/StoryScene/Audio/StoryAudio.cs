using System.Collections;
using System.Collections.Generic;
using EasyTools;
using UnityEngine;

namespace StoryScene {

	internal static class StoryAudio {
		private static GameObject _obj;
		private static AudioSource _sfxSource, _bgmSource;

		internal static void PlaySFX(AudioClip sfx, float volumeScale = 1f) {
			if (_obj == null) Init();

			if (sfx != null) _sfxSource.PlayOneShot(sfx, volumeScale);
		}

		internal static void FadeBGM(AudioClip bgm) => EasyGameLoop.Do(BGM_Fade(bgm));
		static IEnumerator BGM_Fade(AudioClip targetBGM) {
			if (_obj == null) Init();

			if (targetBGM != _bgmSource.clip) {
				var v = _bgmSource.volume;
				yield return EasyTools.Gradient.Linear(0.2f, d => _bgmSource.volume = Mathf.Lerp(v, 0, d));
				_bgmSource.Stop();
				_bgmSource.clip = targetBGM;
				if (targetBGM != null) {
					_bgmSource.Play();
					yield return EasyTools.Gradient.Linear(0.2f, d => _bgmSource.volume = Mathf.Lerp(0, v, d));
				}
				else {
					_bgmSource.volume = v;
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init() {
			_obj = new GameObject("StorySound");
			_sfxSource = _obj.AddComponent<AudioSource>();
			_bgmSource = _obj.AddComponent<AudioSource>();
			_bgmSource.playOnAwake = false;
			_bgmSource.loop = true;
			Object.DontDestroyOnLoad(_obj);
		}
	}
}

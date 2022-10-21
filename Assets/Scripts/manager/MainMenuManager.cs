using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using EasyTools;
using NaughtyAttributes;
using StoryScene;
using UnityEngine.SceneManagement;

namespace MainMenu {

	public class MainMenuManager : MonoBehaviour {
		[SerializeField] private VideoPlayer _vidPlayer;
		[SerializeField] private RawImage _vidImg;
		[SerializeField] private GameObject _bg;
		[SerializeField] private AudioClip _buttonSFX;
		[SerializeField, Scene] private string _startScene;

		private static bool _first = true;

		void Awake() {
			if (_first) {
				_first = false;
				_vidPlayer.loopPointReached += _ => VideoAndBgFade().ApplyTo(this);
				_vidPlayer.Play();
			}
			else {
				_bg.SetActive(true);
				_vidImg.enabled = false;
			}
		}
		IEnumerator VideoAndBgFade() {
			_bg.SetActive(true);
			yield return EasyTools.Gradient.Linear(2f, d => _vidImg.SetA(1 - d));
			_vidImg.enabled = false;
		}

		public void StartGame() => EasyGameLoop.Do(StartGameFade());
		IEnumerator StartGameFade() {
			GameAudio.PlaySFX(_buttonSFX);
			yield return TransitionManager.Current.ShowMaskCoroutine();
			yield return SceneManager.LoadSceneAsync(_startScene);
			yield return TransitionManager.Current.HideMaskCoroutine();
		}

		public void ExitGame() => EasyGameLoop.Do(ExitGameFade());
		IEnumerator ExitGameFade() {
			GameAudio.PlaySFX(_buttonSFX);
			yield return TransitionManager.Current.ShowMaskCoroutine(1f);
#if UNITY_EDITOR    //在编辑器模式退出
			UnityEditor.EditorApplication.isPlaying = false;
#else//发布后退出
            Application.Quit();
#endif
		}
	}
}

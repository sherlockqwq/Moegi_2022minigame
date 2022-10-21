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
		[SerializeField, Scene] private string _startScene;

		void Awake() {
			_vidPlayer.loopPointReached += _ => Fade().ApplyTo(this);
		}

		IEnumerator Fade() {
			yield return EasyTools.Gradient.Linear(2f, d => _vidImg.SetA(1 - d));
			_vidImg.enabled = false;
		}

		public void StartGame() => EasyGameLoop.Do(StartGameFade());

		IEnumerator StartGameFade() {
			yield return TransitionManager.Current.ShowMaskCoroutine();
			yield return SceneManager.LoadSceneAsync(_startScene);
			yield return TransitionManager.Current.HideMaskCoroutine();
		}

		public void ExitGame() {
#if UNITY_EDITOR    //在编辑器模式退出
			UnityEditor.EditorApplication.isPlaying = false;
#else//发布后退出
            Application.Quit();
#endif
		}
	}
}

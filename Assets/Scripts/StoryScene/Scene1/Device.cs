using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using NaughtyAttributes;

namespace StoryScene.Scene1 {

	public class Device : PlayerInteractable {
		[SerializeField] private Transform _panel;
		[Scene, SerializeField] private int _toScene;

		private void Start() {
			_panel.localScale = Vector3.zero;
			_panel.gameObject.SetActive(false);
		}

		private int _myPauseId;
		protected override void OnInteract(StoryPlayerController player) => PanelZoom().ApplyTo(this);

		IEnumerator PanelZoom() {
			if (_panel.gameObject.activeInHierarchy) yield break;

			StoryPlayerController.Current.Pause(out _myPauseId);

			_panel.gameObject.SetActive(true);
			yield return EasyTools.Gradient.EaseOut(0.5f, _panel.SetScale);
		}

		private bool _clicked = false;
		public void OnStartButtonClick() {
			if (!_clicked) {
				_clicked = true;
				// 切换场景时此 MonoBehaviour 会被摧毁，需利用 EasyGameLoop 挂载协程
				EasyGameLoop.Do(SwitchScene());
			}
		}

		IEnumerator SwitchScene() {
			yield return TransitionManager.Current.ShowMaskCoroutine();

			yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_toScene);

			yield return TransitionManager.Current.HideMaskCoroutine();

			StoryPlayerController.Current.Resume(_myPauseId);
		}
	}
}

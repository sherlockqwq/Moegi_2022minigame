using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using NaughtyAttributes;

namespace StoryScene.Scene1 {

	public class Device : PlayerInteractable {
		[SerializeField] private Transform _panel;
		[SerializeField, Scene] private string _toScene;

		private void Start() {
			_panel.localScale = Vector3.zero;
			_panel.gameObject.SetActive(false);
		}

		private int _myPauseId;
		protected override void OnInteract(StoryPlayerController player) => PanelZoom().ApplyTo(this);

		IEnumerator PanelZoom() {
			if (_panel.gameObject.activeInHierarchy) yield break;

			StoryPlayerController.Pause(out _myPauseId);

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
			yield return TransitionManager.Current.LoadSceneCoroutine(_toScene);

			StoryPlayerController.Resume(_myPauseId);
		}
	}
}

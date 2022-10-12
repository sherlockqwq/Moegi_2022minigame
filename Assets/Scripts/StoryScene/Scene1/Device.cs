using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene.Scene1 {

	public class Device : PlayerInteractable {
		[SerializeField] private Transform _panel;

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
			if (_clicked) return;

			_clicked = true;
			// TODO 场景切换

			StoryPlayerController.Current.Resume(_myPauseId);
		}
	}
}

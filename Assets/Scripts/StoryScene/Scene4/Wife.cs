using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace StoryScene.Scene4 {

	public class Wife : PlayerInteractable {
		[SerializeField] private ReplaceAndDialog _required;
		[SerializeField] private SpriteRenderer _standSp, _downSp;
		[SerializeField] private Transform _stand;
		[SerializeField] private GameObject _confirmPanel;
		[SerializeField] private VideoPlayer _videoPlayer;
		[SerializeField, Scene] private string _endScene;
		[SerializeField] private Transform _2ndPos;

		private IEnumerator Start() {
			_videoPlayer.gameObject.SetActive(false);
			_confirmPanel.SetActive(false);

			_standSp.SetA(1);
			_downSp.SetA(0);
			_replaceTip.gameObject.SetActive(false);

			SetActive(false);
			// 等待前置交互完成
			yield return Wait.Until(() => _required.Finished);
			SetActive(true);
		}

		private bool _replaceable = true;
		public override bool Replaceable => _replaceable;

		private bool _firstTouch = true;
		protected override void OnPlayerTouch() {
			base.OnPlayerTouch();
			if (_firstTouch) {
				_firstTouch = false;
				Floating().ApplyTo(this);
			}
		}

		IEnumerator Floating() {
			StoryPlayerController.Pause(out var id);
			yield return StoryPlayerController.Current.ShowFloating();
			StoryPlayerController.Resume(id);
			_replaceTip.gameObject.SetActive(true);
		}

		private int _pauseId;
		private bool _confirmed = false;
		protected override void OnInteract(StoryPlayerController player) {
			if (!_confirmed) FirstDialog().ApplyTo(this);
			else SecondDialog().ApplyTo(this);
		}

		IEnumerator FirstDialog() {

			StoryPlayerController.Pause(out _pauseId);
			yield return Wait.Seconds(2f);
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "TryDelete");

			if (StoryPlayerController.Current.transform.position.x < _stand.position.x) {
				yield return EasyTools.Gradient.EaseInOut(1f, d => _stand.localScale = new Vector3(1 - 2 * d, 1, 1));
			}
			else yield return Wait.Seconds(1f);

			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "DeleteDialog");

			_confirmPanel.SetActive(true);
			_confirmed = true;
		}


		public void ConfirmDelete() => Delete().ApplyTo(this);
		IEnumerator Delete() {
			_confirmPanel.SetActive(false);
			yield return EasyTools.Gradient.Linear(0.5f, d => _standSp.SetA(1 - d));
			transform.position = _2ndPos.position;
			_replaceable = false;
			yield return EasyTools.Gradient.Linear(0.5f, _downSp.SetA);
			StoryPlayerController.Resume(_pauseId);
		}
		IEnumerator SecondDialog() {
			SetActive(false);
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "ConfirmDelete");
			yield return ShowEnding(
				EasyLocalization.Get<string[]>("Ending", "Deleted"),
				EasyLocalization.Get<string>("Ending", "DeletedEnding")
			);
		}

		public void CancelDelete() => Cancel().ApplyTo(this);
		IEnumerator Cancel() {
			SetActive(false);

			_confirmPanel.SetActive(false);
			StoryPlayerController.Resume(_pauseId);
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "CancelDelete");

			yield return ShowEnding(
				EasyLocalization.Get<string[]>("Ending", "Kept"),
				EasyLocalization.Get<string>("Ending", "KeptEnding")
			);
		}


		IEnumerator ShowEnding(string[] texts, string ending) {
			yield return TransitionManager.Current.ShowMaskCoroutine(1f);

			StoryPlayerController.Pause(out _pauseId);

			yield return Wait.Seconds(1f);

			TransitionManager.Current.MaskText.text = "";
			TransitionManager.Current.MaskText.SetA(1);

			foreach (var text in texts) {   // 打字机效果
				foreach (var ch in text) {
					TransitionManager.Current.MaskText.text += ch;
					yield return Wait.Seconds(0.1f);
				}
				yield return Wait.Seconds(0.5f);
				TransitionManager.Current.MaskText.text += '\n';
			}
			yield return Wait.Seconds(1.5f);    // 等待一段时间
			yield return EasyTools.Gradient.Linear(1f, d => TransitionManager.Current.MaskText.SetA(1 - d));    // 淡出

			GameAudio.FadeBGM(null, 3f);    // 淡出背景音乐

			TransitionManager.Current.MaskText.text = ending;   // 切换为结局文字
			yield return EasyTools.Gradient.Linear(1f, TransitionManager.Current.MaskText.SetA);    // 淡入
			yield return Wait.Seconds(2f);  // 等待一段时间
			yield return EasyTools.Gradient.Linear(1f, d => TransitionManager.Current.MaskText.SetA(1 - d));    // 淡出

			// 设置视频
			_videoPlayer.gameObject.SetActive(true);
			_videoPlayer.loopPointReached += _ => {
				StoryPlayerController.Resume(_pauseId);
				TransitionManager.Current.LoadScene(_endScene, 1f);
			};
			_videoPlayer.Prepare();
			yield return Wait.Until(() => _videoPlayer.isPrepared);
			_videoPlayer.Play();
			yield return TransitionManager.Current.HideMaskCoroutine(1f);
		}
	}

}

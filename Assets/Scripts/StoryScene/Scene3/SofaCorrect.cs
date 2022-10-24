using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyTools;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StoryScene.Scene3 {

	public class SofaCorrect : PlayerInteractable {
		[SerializeField] private ReplaceAndDialog[] _required;
		[SerializeField] private Image _letter;
		[SerializeField] private Sprite[] _letters;
		[SerializeField] private KeyCode[] _nextKeys = new KeyCode[] { KeyCode.Mouse0 };
		[SerializeField] private AudioClip _paperSound;
		[SerializeField] private float _finishDelay = 3f;
		[SerializeField, Scene] private string _nextScene;
		[SerializeField] private AudioClip _transitionSound;

		private IEnumerator Start() {
			_letter.SetA(0);

			if (_required.Length > 0) {
				SetActive(false);
				// 等待前置交互完成
				yield return Wait.Until(() => _required.All(item => item.Finished));
				SetActive(true);
			}
		}

		protected override void OnInteract(StoryPlayerController player) {
			SetActive(false);
			EasyGameLoop.Do(C());
		}

		IEnumerator C() {
			StoryPlayerController.Pause(out var id);

			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story3_Dialog", "Sofa_Correct");

			yield return EasyTools.Gradient.Linear(0.5f, _letter.SetA); // 显示信件
			GameAudio.PlaySFX(_paperSound);
			foreach (var letter in _letters) {
				_letter.sprite = letter;
				yield return Wait.Seconds(0.5f);
				yield return Wait.Until(() => _nextKeys.Any(key => Input.GetKeyDown(key)));
				GameAudio.PlaySFX(_paperSound);
			}
			yield return EasyTools.Gradient.Linear(0.5f, d => _letter.SetA(1 - d)); // 隐藏信件

			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story3_Dialog", "Sofa_Finished");

			StoryPlayerController.Resume(id);	// 恢复控制

			StoryPlayerController.Current.CanInteract = false;	// 禁止交互

			yield return Wait.Seconds(_finishDelay);

			StoryPlayerController.Current.CanInteract = true;	// 允许交互

			StoryPlayerController.Pause(out id);	// 暂停控制

			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story3_Dialog", "Finished");

			GameAudio.PlaySFX(_transitionSound);

			yield return TransitionManager.Current.ShowMaskCoroutine(1.5f);

			yield return SceneManager.LoadSceneAsync(_nextScene);

			yield return TransitionManager.Current.HideMaskCoroutine(1.5f);

			StoryPlayerController.Resume(id);

		}
	}
}

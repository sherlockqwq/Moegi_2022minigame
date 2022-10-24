using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

namespace StoryScene {

	public class RepairManager : MonoBehaviour {
		[SerializeField] private ReplaceAndDialog[] _required;
		[SerializeField] private string _finishDialogFile, _finishDialogKey;
		[SerializeField, Scene] private string _nextScene;
		[SerializeField] private AudioClip _transitionSound;
		[SerializeField] private float _finishDelay = 2f;

		void Start() {
			EasyGameLoop.Do(C());
		}

		IEnumerator C() {
			yield return Wait.Until(() => _required.All(item => item.Finished));    // 等待全部完成

			StoryPlayerController.Current.CanInteract = false;  // 禁止交互

			yield return Wait.Seconds(_finishDelay);

			StoryPlayerController.Current.CanInteract = true;   // 允许交互

			StoryPlayerController.Pause(out var id);	// 暂停控制

			yield return DialogManager.Current.ShowEasyLocalizationAndWait(_finishDialogFile, _finishDialogKey);

			GameAudio.PlaySFX(_transitionSound);

			yield return TransitionManager.Current.ShowMaskCoroutine(1.5f);

			yield return SceneManager.LoadSceneAsync(_nextScene);

			yield return TransitionManager.Current.HideMaskCoroutine(1.5f);

			StoryPlayerController.Resume(id);
		}
	}
}

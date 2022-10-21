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

		void Start() {
			EasyGameLoop.Do(C());
		}

		IEnumerator C() {
			yield return Wait.Until(() => _required.All(item => item.Finished));

			StoryPlayerController.Pause(out var id);

			yield return DialogManager.Current.ShowEasyLocalizationAndWait(_finishDialogFile, _finishDialogKey);

			GameAudio.PlaySFX(_transitionSound);

			yield return TransitionManager.Current.ShowMaskCoroutine(1.5f);

			yield return SceneManager.LoadSceneAsync(_nextScene);

			yield return TransitionManager.Current.HideMaskCoroutine(1.5f);

			StoryPlayerController.Resume(id);
		}
	}
}

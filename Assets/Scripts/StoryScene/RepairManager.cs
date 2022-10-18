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
		[SerializeField, Scene] private int _nextScene;

		void Start() {
			EasyGameLoop.Do(C());
		}

		IEnumerator C() {
			yield return Wait.Until(() => _required.All(item => item.Finished));
			DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_finishDialogFile, _finishDialogKey));
			yield return Wait.Until(() => !DialogManager.Showing);

			yield return TransitionManager.Current.ShowMaskCoroutine();

			yield return SceneManager.LoadSceneAsync(_nextScene);

			yield return TransitionManager.Current.HideMaskCoroutine();
		}
	}
}

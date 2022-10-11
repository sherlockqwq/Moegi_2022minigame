using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using UnityEngine.Events;

namespace StoryScene {

	public class SimpleDialog : MonoBehaviour, IPlayerInteractable {
		[SerializeField] private string _fileName, _keyName;

		[SerializeField] private UnityEvent<bool> _onInteractableChange;

		void Awake() {
			_onInteractableChange.Invoke(false);
		}

		void IPlayerInteractable.OnPlayerLeave(StoryPlayerController player) => _onInteractableChange.Invoke(false);

		void IPlayerInteractable.OnPlayerTouch(StoryPlayerController player) => _onInteractableChange.Invoke(true);

		void IPlayerInteractable.OnInteract(StoryPlayerController player) {
			DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_fileName, _keyName));
		}
	}
}

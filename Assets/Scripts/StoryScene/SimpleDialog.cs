using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene {

	public class SimpleDialog : MonoBehaviour, IPlayerInteractable {
		[SerializeField] private string _fileName, _keyName;

		void IPlayerInteractable.Interact(StoryPlayerController player) {
			DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_fileName, _keyName));
		}
	}
}

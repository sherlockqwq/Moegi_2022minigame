using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using UnityEngine.Events;

namespace StoryScene {

	public class SimpleDialog : PlayerInteractable {
		[SerializeField] private string _fileName, _keyName;

		protected override void OnInteract(StoryPlayerController player) {
			DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_fileName, _keyName));
		}
	}
}

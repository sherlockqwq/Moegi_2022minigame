using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using UnityEngine.Events;

namespace StoryScene {

	/// <summary>
	/// 交互并进行一段对话的简单的物体，使用到 EasyLocalization
	/// </summary>
	public class SimpleDialog : PlayerInteractable {
		[SerializeField] private string _fileName, _keyName;

		protected override void OnInteract(StoryPlayerController player) {
			DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_fileName, _keyName));
		}
	}
}

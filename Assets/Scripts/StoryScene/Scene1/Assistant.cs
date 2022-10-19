using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene.Scene1 {

	public class Assistant : PlayerInteractable {

		[SerializeField] private Transform _assistantModel;
		[SerializeField] private Sprite _2ndSprite;
		[SerializeField] private PlayerInteractable _device;

		protected void Start() {
			_device.SetActive(false);
		}

		protected override void OnInteract(StoryPlayerController player) {
			Story().ApplyTo(this);
		}

		IEnumerator Story() {
			StoryPlayerController.Current.Pause(out var id);
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story1_Dialog", "Assistant1");

			yield return EasyTools.Gradient.EaseIn(0.2f, d => _assistantModel.localScale = new Vector3(1 - d, 1, 1));
			_assistantModel.GetComponentInChildren<SpriteRenderer>(true).sprite = _2ndSprite;
			yield return EasyTools.Gradient.EaseOut(0.2f, d => _assistantModel.localScale = new Vector3(-d, 1, 1));

			yield return Wait.Seconds(0.3f);

			yield return EasyTools.Gradient.EaseInOut(0.4f, d => _assistantModel.localScale = new Vector3(2 * d - 1, 1, 1));
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story1_Dialog", "Assistant2");

			StoryPlayerController.Current.Resume(id);
			_device.SetActive(true);
			SetActive(false);
		}
	}
}

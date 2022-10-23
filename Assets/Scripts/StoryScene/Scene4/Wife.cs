using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;

namespace StoryScene.Scene4 {

	public class Wife : PlayerInteractable {
		[SerializeField] private ReplaceAndDialog _required;
		[SerializeField] private SpriteRenderer _standSp, _downSp;
		[SerializeField] private Transform _stand;

		private IEnumerator Start() {
			_standSp.SetA(1);
			_downSp.SetA(0);
			_replaceTip.gameObject.SetActive(false);

			SetActive(false);
			// 等待前置交互完成
			yield return Wait.Until(() => _required.Finished);
			SetActive(true);
		}

		public override bool Replaceable => true;

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

		protected override void OnInteract(StoryPlayerController player) => C().ApplyTo(this);

		IEnumerator C() {
			StoryPlayerController.Pause(out _pauseId);
			yield return Wait.Seconds(2f);
			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "TryDelete").ApplyTo(this);

			if (StoryPlayerController.Current.transform.position.x > _stand.position.x) {
				yield return EasyTools.Gradient.EaseInOut(1f, d => _stand.localScale = new Vector3(1 - 2 * d, 1, 1));
			}
			else yield return Wait.Seconds(1f);

			yield return DialogManager.Current.ShowEasyLocalizationAndWait("Story4_Dialog", "DeleteDialog").ApplyTo(this);

			// TODO　选择
		}
	}
}

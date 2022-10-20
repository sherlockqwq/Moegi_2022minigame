using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using EasyTools;
using System.Linq;
using UnityEngine.Events;

namespace StoryScene {

	public class ReplaceAndDialog : PlayerInteractable {
		[SerializeField] private ReplaceAndDialog[] _required;

		[SerializeField, Header("首次交互")] private bool _canFirstInteract;
		[SerializeField, ShowIf(nameof(_canFirstInteract))] private string _firstDialogFile, _firstDialogKey;
		[SerializeField, ShowIf(nameof(_canFirstInteract))] private bool _showFloating = true;

		[SerializeField, Header("替换")] private bool _canReplace;
		[SerializeField, ShowIf(nameof(_canReplace))] private SpriteRenderer[] _spritesToFadeOut;
		[SerializeField, ShowIf(nameof(_canReplace))] private SpriteRenderer[] _spritesToFadeIn;
		[SerializeField, ShowIf(nameof(_canReplace))] private bool _dialogAfterReplace;

		[SerializeField, ShowIf(EConditionOperator.And, nameof(_canReplace), nameof(_dialogAfterReplace))]
		private string _replacedDialogFile, _replacedDialogKey;
		[SerializeField, ShowIf(nameof(_canReplace))] private UnityEvent _afterReplace;

		[SerializeField, Header("最终无限次交互")] private bool _canFinalInteract;
		[SerializeField, ShowIf(nameof(_canFinalInteract))] private string _finalDialogFile, _finalDialogKey;

		public bool Finished { get; private set; } = false;
		private bool _replaceable = false;
		public override bool Replaceable => _replaceable;

		private IEnumerator Start() {
			if (_canReplace) {
				_spritesToFadeOut.Each(sp => sp.SetA(1));
				_spritesToFadeIn.Each(sp => sp.SetA(0));
			}

			if (!_canFirstInteract) UpdateProgress();

			if (_required.Length > 0) {
				SetActive(false);
				// 等待前置交互完成
				yield return Wait.Until(() => _required.All(item => item.Finished));
				SetActive(true);
			}
		}

		protected override void OnInteract(StoryPlayerController player) => Interact().ApplyTo(this);

		IEnumerator Interact() {
			StoryPlayerController.Pause(out var id);

			switch (_progress) {
				case Progress.First:
					yield return DialogManager.Current.ShowEasyLocalizationAndWait(_firstDialogFile, _firstDialogKey);
					if (_showFloating) yield return StoryPlayerController.Current.ShowFloating();
					break;

				case Progress.Replace:
					if (_spritesToFadeOut.Length > 0)
						yield return EasyTools.Gradient.Linear(1f, d => _spritesToFadeOut.Each(sp => sp.SetA(1 - d)));
					if (_spritesToFadeIn.Length > 0)
						yield return EasyTools.Gradient.Linear(1f, d => _spritesToFadeIn.Each(sp => sp.SetA(d)));
					_afterReplace?.Invoke();
					if (_dialogAfterReplace)
						yield return DialogManager.Current.ShowEasyLocalizationAndWait(_replacedDialogFile, _replacedDialogKey);

					break;

				case Progress.Final:
					yield return DialogManager.Current.ShowEasyLocalizationAndWait(_finalDialogFile, _finalDialogKey);
					Finished = true;
					break;
			}

			UpdateProgress();
			StoryPlayerController.Resume(id);
		}


		private enum Progress {
			First, Replace, Final, Disabled
		}
		private Progress _progress = Progress.First;
		private void UpdateProgress() {
			switch (_progress) {
				case Progress.First:
					if (_canReplace) {
						_progress = Progress.Replace;
						_replaceable = true;
					}
					else goto case Progress.Replace;
					break;

				case Progress.Replace:
					_replaceable = false;
					if (_canFinalInteract) _progress = Progress.Final;
					else {
						_progress = Progress.Disabled;
						SetActive(false);
						Finished = true;
					}
					break;
			}
		}

	}
}

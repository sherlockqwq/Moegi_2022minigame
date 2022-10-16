using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using EasyTools;

namespace StoryScene {

	public class ReplaceAndDialog : PlayerInteractable {
		[Header("替换流程")]
		[SerializeField] private bool _firstDialog;
		[SerializeField] private bool _fadeOut, _fadeIn, _autoDialog, _nextDialog;

		[SerializeField, ShowIf(nameof(_firstDialog))] private string _firstDialogFile, _firstDialogKey;
		[SerializeField, ShowIf(nameof(_fadeOut))] private SpriteRenderer _spriteToFadeOut;
		[SerializeField, ShowIf(nameof(_fadeIn))] private SpriteRenderer _spriteToFadeIn;
		[SerializeField, ShowIf(nameof(_autoDialog))] private string _autoDialogFile, _autoDialogKey;
		[SerializeField, ShowIf(nameof(_nextDialog))] private string _nextDialogFile, _nextDialogKey;

		public bool Finished { get; private set; } = false;
		private bool _replaceable = false;
		public override bool Replaceable => _replaceable;

		private void Start() {
			if (!_firstDialog) {
				_replaceable = true;
				_progress = 1;
			}
		}

		protected override void OnInteract(StoryPlayerController player) {
			Progress().ApplyTo(this);
		}

		private byte _progress = 0;
		IEnumerator Progress() {
			StoryPlayerController.Current.Pause(out var id);
			switch (_progress) {
				case 0: // 首次 E 交互
					DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_firstDialogFile, _firstDialogKey));
					yield return Wait.Until(() => DialogManager.Showing);
					// TODO 飘字
					_replaceable = true;
					break;
				case 1: // Q 交互
					if (_fadeOut) yield return EasyTools.Gradient.Linear(1f, d => _spriteToFadeOut.SetA(1 - d));
					if (_fadeIn) yield return EasyTools.Gradient.Linear(1f, _spriteToFadeIn.SetA);
					if (_autoDialog) {
						DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_autoDialogFile, _autoDialogKey));
						yield return Wait.Until(() => DialogManager.Showing);
					}
					if (_nextDialog) {
						_replaceable = false;
					}
					else {
						SetActive(false);
						Finished = true;
					}
					break;
				default:    // 再次 E 交互
					if (_nextDialog) {
						DialogManager.Current.Show(EasyLocalization.Get<DialogMsg[]>(_nextDialogFile, _nextDialogKey));
						yield return Wait.Until(() => DialogManager.Showing);
						Finished = true;
					}
					break;
			}
			if (_progress < 2) _progress++;
			StoryPlayerController.Current.Resume(id);
		}
	}
}

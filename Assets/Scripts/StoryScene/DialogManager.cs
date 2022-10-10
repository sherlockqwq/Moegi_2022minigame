using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyTools;
using NaughtyAttributes;
using System;
using Newtonsoft.Json;

namespace StoryScene {

	public class DialogManager : MonoBehaviour {

		public static bool Showing { get; private set; } = false;
		public static DialogManager Current { get; private set; }

		[SerializeField] private GameObject _panel;
		[SerializeField] private Image _avatarImg;
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _contentText;
		[SerializeField] private AudioClip _sfx;
		[SerializeField] private KeyCode[] _nextKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
		[SerializeField] private KeyCode[] _skipKeys = new KeyCode[] { KeyCode.LeftControl };

		// TODO 这里序列化各种立绘
		[SerializeField] private Sprite _doctorAvatar;

		private void SetSpeaker(DialogMsg message) {
			_avatarImg.sprite = message.avatar.ToLower() switch {
				"doctor" => _doctorAvatar,
				_ => null
			};
			if (_avatarImg.sprite != null) {
				_avatarImg.enabled = true;
				_avatarImg.SetNativeSize();
			}
			else _avatarImg.enabled = false;
			_nameText.text = message.name;
		}

		private Queue<DialogMsg> dialogues = new Queue<DialogMsg>();

		private void Awake() {
			Current = this;
			_contentText = _panel.GetComponentInChildren<TMP_Text>();
			_panel.SetActive(false);
		}

		public void Show(params DialogMsg[] messages) => DelayShow(0, messages);
		public void DelayShow(float delay, params DialogMsg[] messages) {
			foreach (var dialogue in messages) {
				dialogues.Enqueue(dialogue);
			}
			if (!Showing && dialogues.Count > 0) ShowDialogC(delay).ApplyTo(this);
		}

		private int myPauseId;
		IEnumerator ShowDialogC(float delay) {
			Showing = true;

			yield return Wait.Seconds(delay);

			_panel.SetActive(true);
			while (dialogues.TryDequeue(out var message)) {
				SetSpeaker(message);

				_contentText.text = "";
				yield return null;
				StartSFX();

				for (int count = 0; count < message.content.Length; count++) {
					yield return Wait.Seconds(0.05f, CanSkip);

					if (CanSkip()) {
						_contentText.text = message.content;
						yield return null;
						break;
					}
					_contentText.text += message.content[count];
				}

				StopSFX();
				yield return Wait.Until(CanSkip);
			}
			Showing = false;
			Hide();
		}

		private bool CanSkip() => _nextKeys.Any(c => Input.GetKeyDown(c)) || _skipKeys.Any(c => Input.GetKey(c));

		public void Hide() {
			StopSFX();

			_panel.SetActive(false);
			if (Showing) {
				StopAllCoroutines();
				dialogues.Clear();
				Showing = false;
			}
		}

		public void ShowAsFloat(DialogMsg message) {
			SetSpeaker(message);
			_contentText.text = message.content;
			_panel.SetActive(true);
			_panel.GetComponent<Image>().enabled = false;
		}

		public void HideFloat() {
			_panel.GetComponent<Image>().enabled = true;
			_panel.SetActive(false);
		}

		#region 音效相关

		private AudioSource _audio;

		private void StartSFX() {
			if (_audio == null) {
				if (!TryGetComponent<AudioSource>(out _audio)) {
					_audio = gameObject.AddComponent<AudioSource>();
				}
				_audio.playOnAwake = false;
				_audio.Stop();
			}
			_dialogCoroutine = PlaySFX().ApplyTo(this);
		}
		private void StopSFX() {
			if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
		}
		Coroutine _dialogCoroutine;
		WaitForSeconds _wait = new WaitForSeconds(0.08f);
		IEnumerator PlaySFX() {
			while (true) {
				_audio.PlayOneShot(_audio.clip);
				yield return _wait;
			}
		}

		#endregion
	}

	public class DialogMsg {
		public string avatar = string.Empty, name = string.Empty, content = string.Empty, position = "left";
	}
}

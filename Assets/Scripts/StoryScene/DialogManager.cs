using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyTools;

namespace StoryScene {

	public class DialogManager : MonoBehaviour {
		public static bool Showing { get; private set; } = false;

		[SerializeField] private GameObject panel;
		[SerializeField] private Image avatar;

		// TODO 这里序列化各种立绘

		[SerializeField] private AudioClip _sfx;
		private TMP_Text tmp;

		private struct Dialogue { public string from, content; }
		private Queue<(string From, string Content)> dialogues = new Queue<(string, string)>();


		private void Awake() {
			tmp = panel.GetComponentInChildren<TMP_Text>();
			panel.SetActive(false);
		}

		private Sprite GetAvatar(string from) {
			return from.ToLower() switch {
				_ => null
			};
		}

		public void Show(params DialogMsg[] messages) => DelayShow(0, messages);
		public void DelayShow(float delay, params DialogMsg[] messages) {
			foreach (var dialogue in messages) {
				if (!dialogue.IsEmpty) dialogues.Enqueue(dialogue.Content);
			}
			if (!Showing && dialogues.Count > 0) StartCoroutine(ShowDialogC(delay));
		}

		private int myPauseId;
		IEnumerator ShowDialogC(float delay) {
			Showing = true;
			StoryPlayerController.Pause(out myPauseId);

			yield return Wait.Seconds(delay);

			panel.SetActive(true);
			while (dialogues.TryDequeue(out var dialogue)) {
				avatar.sprite = GetAvatar(dialogue.From);
				avatar.enabled = avatar.sprite != null;

				tmp.text = "";
				yield return null;
				StartSFX();
				for (int count = 0; count < dialogue.Content.Length; count++) {
					yield return Wait.Seconds(0.05f, CanSkip);

					if (CanSkip()) {
						tmp.text = dialogue.Content;
						yield return null;
						break;
					}
					tmp.text += dialogue.Content[count];
				}
				StopSFX();
				yield return Wait.Until(CanSkip);
			}
			Showing = false;
			Hide();
		}

		private bool CanSkip() => Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.LeftControl);

		public void Hide() {
			StopSFX();

			panel.SetActive(false);
			StoryPlayerController.Resume(myPauseId);
			if (Showing) {
				StopAllCoroutines();
				dialogues.Clear();
				Showing = false;
			}
		}

		public void ShowAsFloat(DialogMsg message) {
			avatar.sprite = GetAvatar(message.Content.From);
			avatar.enabled = avatar.sprite != null;
			tmp.text = message.Content.Content;
			panel.SetActive(true);
			panel.GetComponent<Image>().enabled = false;
		}

		public void HideFloat() {
			panel.GetComponent<Image>().enabled = true;
			panel.SetActive(false);
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

	public class DialogMsg : Dictionary<string, string> {
		public bool IsEmpty => base.Count <= 0;
		public (string From, string Content) Content => (base.Keys.First(), base[base.Keys.First()]);
	}
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyTools;

namespace StoryScene {

	/// <summary>
	/// 对话框管理器（单例 Current）
	/// </summary>
	public class DialogManager : MonoBehaviour {
		public static DialogManager Current { get; private set; }

		public bool Showing { get; private set; } = false;

		[SerializeField] private GameObject _panel;
		[SerializeField] private Image _avatarImg;
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _contentText;
		[SerializeField] private AudioClip _sfx;
		[SerializeField] private KeyCode[] _nextKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
		[SerializeField] private KeyCode[] _skipKeys = new KeyCode[] { KeyCode.LeftControl };

		// TODO 这里序列化各种立绘
		[SerializeField] private Sprite _doctorAvatar, _doctorSadAvatar, _sonAvatar, _sonSadAvatar, _wifeAvatar;

		private void SetSpeaker(DialogMsg message) {
			_avatarImg.sprite = message.avatar.ToLower() switch {
				"doctor" => _doctorAvatar,
				"doctor_sad" => _doctorSadAvatar,
				"son" => _sonAvatar,
				"son_sad" => _sonSadAvatar,
				"wife" => _wifeAvatar,
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

			_panel.SetActive(false);
			InitAudio();
		}

		/// <summary>
		/// 显示一串对话内容
		/// </summary>
		public void Show(params DialogMsg[] messages) => ShowAndWait(messages).ApplyTo(this);

		/// <summary>
		/// 通过 EasyLocalization 获取对话并显示（协程方法）
		/// </summary>
		public IEnumerator ShowEasyLocalizationAndWait(string fileName, string key) =>
			ShowAndWait(EasyLocalization.Get<DialogMsg[]>(fileName, key));

		/// <summary>
		/// 显示一串对话内容（协程方法）
		/// </summary>
		public IEnumerator ShowAndWait(params DialogMsg[] messages) {
			if (messages == null) yield break;
			foreach (var dialogue in messages) {
				dialogues.Enqueue(dialogue);
			}
			if (!Showing && dialogues.Count > 0) yield return ShowDialogC();
		}

		private int myPauseId;
		// 显示对话的协程
		IEnumerator ShowDialogC() {
			Showing = true;
			StoryPlayerController.Pause(out myPauseId);

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

		/// <summary>
		/// 隐藏对话框
		/// </summary>
		public void Hide() {
			StopSFX();

			_panel.SetActive(false);
			StoryPlayerController.Resume(myPauseId);
			if (Showing) {
				StopAllCoroutines();
				dialogues.Clear();
				Showing = false;
			}
		}

		/// <summary>
		/// 以悬浮窗方式显示一条对话（不影响点击、移动等）
		/// </summary>
		public void ShowAsFloat(DialogMsg message) {
			SetSpeaker(message);
			_contentText.text = message.content;
			_panel.SetActive(true);
			_panel.GetComponent<Image>().enabled = false;
		}

		/// <summary>
		/// 隐藏以悬浮窗方式显示的对话框
		/// </summary>
		public void HideFloat() {
			_panel.GetComponent<Image>().enabled = true;
			_panel.SetActive(false);
		}

		#region 音效相关

		private AudioSource _audio;

		private void InitAudio() {
			if (!TryGetComponent<AudioSource>(out _audio)) {
				_audio = gameObject.AddComponent<AudioSource>();
			}
			_audio.playOnAwake = false;
			_audio.Stop();
		}

		private void StartSFX() {
			if (_sfx != null) _dialogCoroutine = PlaySFX().ApplyTo(this);
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

	/// <summary>
	/// 一条对话内容
	/// </summary>
	public class DialogMsg {
		/// <summary> 立绘 </summary>
		public string avatar = string.Empty;
		/// <summary> 显示的名称 </summary>
		public string name = string.Empty;
		/// <summary> 对话内容 </summary>
		public string content = string.Empty;
		/// <summary> 立绘位置 </summary>
		public string position = "left";
	}
}

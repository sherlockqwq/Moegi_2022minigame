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
using UnityEditor;

namespace StoryScene {

	public class DialogManager : MonoBehaviour {

		#region 实例化

		private static string _prefabPath = "Assets/Prefab/StoryScene/Dialog.prefab";
		[RuntimeInitializeOnLoadMethod]
		private static void InitSelf() {
			var obj = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
			if (obj != null && obj.TryGetComponent<DialogManager>(out _)) {
				obj = Instantiate(obj);
				DontDestroyOnLoad(obj);
				_instance = obj.GetComponent<DialogManager>();
			}
			else Debug.LogError("DialogManager初始化失败！请确认 Prefab 路径是否正确！");
		}

		#endregion

		public static bool Showing { get; private set; } = false;

		private static DialogManager _instance;
		public static DialogManager Current {
			get {
				if (_instance == null) throw new NullReferenceException("DialogManager已被摧毁！");
				else return _instance;
			}
			private set => _instance = value;
		}

		[SerializeField] private GameObject _panel;
		[SerializeField] private Image _avatarImg;
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _contentText;
		[SerializeField] private AudioClip _sfx;
		[SerializeField] private KeyCode[] _nextKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
		[SerializeField] private KeyCode[] _skipKeys = new KeyCode[] { KeyCode.LeftControl };

		// TODO 这里序列化各种立绘
		[SerializeField] private Sprite _doctorAvatar, _sonAvatar, _wifeAvatar;

		private void SetSpeaker(DialogMsg message) {
			_avatarImg.sprite = message.avatar.ToLower() switch {
				"doctor" => _doctorAvatar,
				"son" => _sonAvatar,
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
			_panel.SetActive(false);
			InitAudio();
		}

		/// <summary>
		/// 显示一串对话内容
		/// </summary>
		public void Show(params DialogMsg[] messages) => DelayShow(0, messages);

		/// <summary>
		/// 在延时一段时间后显示一串对话内容
		/// </summary>
		public void DelayShow(float delay, params DialogMsg[] messages) {
			foreach (var dialogue in messages) {
				dialogues.Enqueue(dialogue);
			}
			if (!Showing && dialogues.Count > 0) ShowDialogC(delay).ApplyTo(this);
		}

		// 显示对话的协程
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

		/// <summary>
		/// 隐藏对话框
		/// </summary>
		public void Hide() {
			StopSFX();

			_panel.SetActive(false);
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

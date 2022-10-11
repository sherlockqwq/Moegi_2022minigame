using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using EasyTools;

namespace StoryScene {

	public class TransitionManager : MonoBehaviour {

		#region 实例化

		private static string _prefabPath = "Assets/Prefab/StoryScene/Transition.prefab";
		[RuntimeInitializeOnLoadMethod]
		private static void InitSelf() {
			var obj = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
			if (obj != null && obj.TryGetComponent<TransitionManager>(out _)) {
				obj = Instantiate(obj);
				DontDestroyOnLoad(obj);
				_instance = obj.GetComponent<TransitionManager>();
				_instance.DisableMask();
			}
			else Debug.LogError("TransitionManager 初始化失败！请确认 Prefab 路径是否正确！");
		}

		#endregion

		private static TransitionManager _instance;
		public static TransitionManager Current {
			get {
				if (_instance == null) throw new NullReferenceException("TransitionManager 已被摧毁！");
				else return _instance;
			}
			private set => _instance = value;
		}

		[SerializeField] private Image _mask;

		public void MaskFadeIn(float fadeTime = 0.5f) => MaskFadeIn(Color.black, fadeTime);
		public void MaskFadeIn(Color maskColor, float fadeTime = 0.5f) {
			StopFade();
			_mask.color = maskColor;
			ShowMaskCoroutine(fadeTime).ApplyTo(this);
		}
		public void MaskFadeOut(float fadeTime = 0.5f) => MaskFadeOut(Color.black, fadeTime);
		public void MaskFadeOut(Color maskColor, float fadeTime = 0.5f) {
			StopFade();
			_mask.color = maskColor;
			HideMaskCoroutine(fadeTime).ApplyTo(this);
		}
		public void StopFade() => StopAllCoroutines();

		public IEnumerator ShowMaskCoroutine(float fadeTime = 0.5f) {
			_mask.enabled = true;
			yield return EasyTools.Gradient.Linear(fadeTime, _mask.SetA);
		}
		public IEnumerator HideMaskCoroutine(float fadeTime = 0.5f) {
			yield return EasyTools.Gradient.Linear(fadeTime, d => _mask.SetA(1 - d));
			_mask.enabled = false;
		}

		public void EnableMask() {
			_mask.enabled = true;
			_mask.SetA(1);
		}
		public void DisableMask() {
			_mask.enabled = false;
		}
	}
}

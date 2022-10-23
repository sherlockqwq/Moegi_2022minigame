using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyTools;

namespace StoryScene {

	/// <summary>
	/// 可以淡入淡出的遮罩（单例 Current）
	/// </summary>
	public class TransitionManager : MonoBehaviour {
		public static TransitionManager Current { get; private set; }

		[SerializeField] private Image _mask;
		[SerializeField] private TMPro.TMP_Text _text;

		public TMPro.TMP_Text MaskText => _text;

		void Awake() {
			Current = this;

			_mask.enabled = false;
		}

		#region 不带颜色

		/// <summary>
		/// 遮罩淡入
		/// </summary>
		public void MaskFadeIn(float fadeTime = 0.5f) => MaskFadeIn(Color.black, fadeTime);

		/// <summary>
		/// 遮罩淡出
		/// </summary>
		public void MaskFadeOut(float fadeTime = 0.5f) => MaskFadeOut(Color.black, fadeTime);

		/// <summary>
		/// 遮罩淡入（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator ShowMaskCoroutine(float fadeTime = 0.5f) => ShowMaskCoroutine(Color.black, fadeTime);

		/// <summary>
		/// 遮罩淡出（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator HideMaskCoroutine(float fadeTime = 0.5f) => HideMaskCoroutine(Color.black, fadeTime);

		#endregion

		#region 带颜色

		/// <summary>
		/// 设置遮罩颜色并淡入
		/// </summary>
		public void MaskFadeIn(Color maskColor, float fadeTime = 0.5f) {
			StopFade();
			ShowMaskCoroutine(maskColor, fadeTime).ApplyTo(this);
		}

		/// <summary>
		/// 设置遮罩颜色并淡出
		/// </summary>
		public void MaskFadeOut(Color maskColor, float fadeTime = 0.5f) {
			StopFade();
			HideMaskCoroutine(maskColor, fadeTime).ApplyTo(this);
		}

		/// <summary>
		/// 遮罩淡入（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator ShowMaskCoroutine(Color maskColor, float fadeTime = 0.5f) {
			SetColor(maskColor);
			_mask.enabled = true;
			yield return EasyTools.Gradient.Linear(fadeTime, _mask.SetA);
		}

		/// <summary>
		/// 遮罩淡出（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator HideMaskCoroutine(Color maskColor, float fadeTime = 0.5f) {
			SetColor(maskColor);
			yield return EasyTools.Gradient.Linear(fadeTime, d => _mask.SetA(1 - d));
			_mask.enabled = false;
		}

		private void SetColor(Color? color) {
			if (!(color is Color c)) c = Color.black;
			c.a = _mask.color.a;
			_mask.color = c;
		}

		#endregion

		/// <summary>
		/// 停止淡入与淡出
		/// </summary>
		public void StopFade() => StopAllCoroutines();

	}
}

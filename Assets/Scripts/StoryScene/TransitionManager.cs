using System.Collections;
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

		void Awake() {
			Current = this;

			_mask.enabled = false;
		}

		/// <summary>
		/// 设置遮罩颜色（注意会始终有效）
		/// </summary>
		public void SetColor(Color color) {
			color.a = _mask.color.a;
			_mask.color = color;
		}

		/// <summary>
		/// 遮罩淡入
		/// </summary>
		public void MaskFadeIn(float fadeTime = 0.5f) {
			StopFade();
			ShowMaskCoroutine(fadeTime).ApplyTo(this);
		}

		/// <summary>
		/// 遮罩淡出
		/// </summary>
		public void MaskFadeOut(float fadeTime = 0.5f) {
			StopFade();
			HideMaskCoroutine(fadeTime).ApplyTo(this);
		}

		/// <summary>
		/// 停止淡入与淡出
		/// </summary>
		public void StopFade() => StopAllCoroutines();

		/// <summary>
		/// 遮罩淡入（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator ShowMaskCoroutine(float fadeTime = 0.5f) {
			_mask.enabled = true;
			yield return EasyTools.Gradient.Linear(fadeTime, _mask.SetA);
		}

		/// <summary>
		/// 遮罩淡出（必须在协程中配合 yield return 使用）
		/// </summary>
		public IEnumerator HideMaskCoroutine(float fadeTime = 0.5f) {
			yield return EasyTools.Gradient.Linear(fadeTime, d => _mask.SetA(1 - d));
			_mask.enabled = false;
		}
	}
}

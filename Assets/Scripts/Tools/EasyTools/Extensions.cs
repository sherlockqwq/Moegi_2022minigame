using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mathf;

namespace EasyTools {

	/// <summary>
	/// 扩展各种方法便于使用
	/// </summary>
	public static class Extensions {
		public static bool TryGetComponentInChildren<T>(this Component self, out T component) {
			component = self.GetComponentInChildren<T>();
			return component != null;
		}
		public static bool TryGetComponentInChildren<T>(this GameObject self, out T component) {
			component = self.GetComponentInChildren<T>();
			return component != null;
		}

		public static void DoOnce(this MonoBehaviour self, Action action, float delay, bool unscaledTime = false) {
			IEnumerator c() {
				yield return Wait.Seconds(delay, unscaledTime);
				action();
			}
			self.StartCoroutine(c());
		}
		public static void DoRepeat(this MonoBehaviour self, Action action, float delay, float interval, bool unscaledTime = false) {
			IEnumerator c() {
				while (true) {
					yield return Wait.Seconds(delay, unscaledTime);
					action();
				}
			}
			self.StartCoroutine(c());
		}

		public static void SetA(this SpriteRenderer self, float a) {
			var c = self.color; c.a = Clamp01(a); self.color = c;
		}
		public static void SetA(this Graphic self, float a) {
			var c = self.color; c.a = Clamp01(a); self.color = c;
		}

		public static void SetZDeg(this Transform self, float z) {
			var v = self.eulerAngles; v.z = z; self.eulerAngles = v;
		}
		public static void SetZDeg(this Quaternion self, float z) {
			var v = self.eulerAngles; v.z = z; self.eulerAngles = v;
		}

		public static void SetScale(this Transform self, float scale) {
			self.localScale = new Vector3(scale, scale, scale);
		}

		public static void SetVX(this Rigidbody2D self, float x) {
			var v = self.velocity; v.x = x; self.velocity = v;
		}

		public static Vector2 Pos2(this Transform self) => self.position;
		public static Vector3 To3(this Vector2 self) => self;
		public static Vector2 To2(this Vector3 self) => self;

		public static void Each<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
			foreach (var item in source) action(item);
		}

		/// <summary>
		/// 相当于 source[index % source.Length]
		/// </summary>
		public static T ChooseLoop<T>(this T[] source, int index) => source[index % source.Length];

		/// <summary>
		/// 相当于 source[index]，但 index 会被自动限制在 [0, source.Length - 1]
		/// </summary>
		public static T ChooseClamp<T>(this T[] source, int index) => source[Mathf.Clamp(index, 0, source.Length - 1)];

		public static Coroutine ApplyTo(this IEnumerator coroutine, MonoBehaviour self) => self.StartCoroutine(coroutine);

		public static Coroutine Delay(this MonoBehaviour self, float seconds, Action onFinished) {
			IEnumerator C() {
				yield return Wait.Seconds(seconds);
				onFinished();
			}
			return self.StartCoroutine(C());
		}
		public static Coroutine DelayFrames(this MonoBehaviour self, int frames, Action onFinished) {
			IEnumerator C() {
				yield return Wait.Frames(frames);
				onFinished();
			}
			return self.StartCoroutine(C());
		}
		public static Coroutine Loop(this MonoBehaviour self, float interval, Action func, float delay = 0f) {
			IEnumerator C() {
				yield return Wait.Seconds(delay);
				while (true) {
					func?.Invoke();
					yield return Wait.Seconds(interval);
				}
			}
			return self.StartCoroutine(C());
		}
		public static Coroutine Loop(this MonoBehaviour self, Action func, float delay = 0f) {
			IEnumerator C() {
				yield return Wait.Seconds(delay);
				while (true) {
					yield return null;
					func?.Invoke();
				}
			}
			return self.StartCoroutine(C());
		}
	}

	/// <summary>
	/// 渐变
	/// </summary>
	public static class Gradient {
		/// <summary>
		/// 线性渐变
		/// </summary>
		/// <param name="time">总时间</param>
		/// <param name="action">每帧使用当前值（0 -> 1）调用</param>
		/// <param name="decrease">若为true，则使用1减当前值调用</param>
		public static IEnumerator Linear(float time, Action<float> action) {
			float t = 0;
			while (t < time) {
				action(t / time);
				yield return null;
				t += Time.deltaTime;
			}
			action(1);
		}
		/// <summary>
		/// 递增渐变（变化速度先慢后快）
		/// </summary>
		/// <param name="time">总时间</param>
		/// <param name="action">每帧使用当前值（0 -> 1）调用</param>
		/// <param name="decrease">若为true，则使用1减当前值调用</param>
		public static IEnumerator EaseIn(float time, Action<float> action) {
			float t = 0;
			while (t < time) {
				action(1 - Cos(PI * t / time / 2));
				yield return null;
				t += Time.deltaTime;
			}
			action(1);
		}
		/// <summary>
		/// 递减渐变（变化速度先快后慢）
		/// </summary>
		/// <param name="time">总时间</param>
		/// <param name="action">每帧使用当前值（0 -> 1）调用</param>
		/// <param name="decrease">若为true，则使用1减当前值调用</param>
		public static IEnumerator EaseOut(float time, Action<float> action) {
			float t = 0;
			while (t < time) {
				action(Sin(PI * t / time / 2));
				yield return null;
				t += Time.deltaTime;
			}
			action(1);
		}
		/// <summary>
		/// 非线性渐变（变化速度慢-快-慢）
		/// </summary>
		/// <param name="time">总时间</param>
		/// <param name="action">每帧使用当前值（0 -> 1）调用</param>
		/// <param name="decrease">若为true，则使用1减当前值调用</param>
		public static IEnumerator EaseInOut(float time, Action<float> action) {
			float t = 0;
			while (t < time) {
				action(-(Cos(PI * t / time) - 1) / 2);
				yield return null;
				t += Time.deltaTime;
			}
			action(1);
		}
	}

	public static class Wait {
		public static WaitForEndOfFrame EndOffFrame = new WaitForEndOfFrame();

		public static IEnumerator Seconds(float seconds, Func<bool> interruption, bool unscaledTime = false) {
			float time = 0;
			while (time < seconds) {
				time += (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
				if (interruption()) break;
				yield return null;
			}
		}
		public static IEnumerator Seconds(float seconds, bool unscaledTime = false) {
			float time = 0;
			while (time < seconds) {
				time += (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
				yield return null;
			}
		}

		public static WaitForSeconds NewSeconds(float seconds) => new WaitForSeconds(seconds);

		public static IEnumerator Frames(int count) {
			for (int i = 0; i < count; i++) {
				yield return null;
			}
		}

		/// <summary>
		/// 等待直到条件为true
		/// </summary>
		/// <param name="condition">条件</param>
		public static IEnumerator Until(Func<bool> condition) {
			while (!condition()) yield return null;
		}
		/// <summary>
		/// 等待直到条件为true
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="timeout">超时（大于0时有效）</param>
		public static IEnumerator Until(Func<bool> condition, float timeout = -1) => Until(condition, null, timeout);
		/// <summary>
		/// 等待直到条件为true
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="then">完成后的回调（参数为花费的时间）</param>
		/// <param name="timeout">超时（大于0时有效）</param>
		public static IEnumerator Until(Func<bool> condition, Action<float> then, float timeout = -1) {
			var t = 0f;
			while (!condition()) {
				yield return null;
				t += Time.deltaTime;
				if (t >= timeout && timeout >= 0) break;
			}
			then?.Invoke(t);
		}
	}
}

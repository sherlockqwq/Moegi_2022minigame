using System.Collections;
using System.Collections.Generic;
using EasyTools;
using UnityEngine;

namespace StoryScene {

	/// <summary>
	/// 剧情场景玩家可交互物体的基类
	/// </summary>
	public abstract class PlayerInteractable : MonoBehaviour {

		[SerializeField] protected SpriteRenderer _outline;
		[SerializeField] private SpriteRenderer _replaceTip;
		[SerializeField] private AudioClip _interactSound;
		private enum LookToPlayerMode {
			None, FlipIsLeft, FlipIsRight,
		}
		[SerializeField] private LookToPlayerMode _lookToPlayerMode;

		/// <summary> 是否是需要替换的物体 </summary>
		public virtual bool Replaceable => false;

		protected virtual void Awake() {
			Leave();

			if ((_lookToPlayerMode == LookToPlayerMode.FlipIsLeft || _lookToPlayerMode == LookToPlayerMode.FlipIsRight)
				&& TryGetComponent<SpriteRenderer>(out var sr)) {
				this.Loop(() => {
					if (StoryPlayerController.Current != null) {
						sr.flipX = StoryPlayerController.Current.transform.position.x > transform.position.x ^
									_lookToPlayerMode == LookToPlayerMode.FlipIsLeft;
					}
				});
			}
		}

		/// <summary>
		/// 与该物体交互
		/// </summary>
		/// <param name="player">与该物体交互的玩家</param>
		public void Interact(StoryPlayerController player) {
			GameAudio.PlaySFX(_interactSound);
			OnInteract(player);
		}

		Coroutine _outlineFadeCoroutine;

		/// <summary>
		/// 玩家靠近该物体
		/// </summary>
		public void Touch() {
			if (_outline != null) {
				EasyGameLoop.Stop(_outlineFadeCoroutine);
				_outlineFadeCoroutine = EasyGameLoop.Do(OutlineFade(1));
			}
			if (Replaceable && _replaceTip != null) _replaceTip.SetA(1);

			OnPlayerTouch();
		}

		/// <summary>
		/// 玩家离开该物体
		/// </summary>
		public void Leave() {
			if (_outline != null) {
				EasyGameLoop.Stop(_outlineFadeCoroutine);
				_outlineFadeCoroutine = EasyGameLoop.Do(OutlineFade(0));
			}
			if (_replaceTip != null) _replaceTip.SetA(0);

			OnPlayerLeave();
		}

		IEnumerator OutlineFade(float target) {
			float start = _outline.color.a;
			float time = Mathf.Abs((target - start) / 10f);
			yield return EasyTools.Gradient.Linear(time, d => _outline.SetA(Mathf.Lerp(start, target, d)));
		}

		/// <summary>
		/// 设置此物体是否可交互
		/// </summary>
		public void SetActive(bool value) {
			if (TryGetComponent<Collider2D>(out var c)) {
				c.enabled = value;
			}
		}

		protected abstract void OnInteract(StoryPlayerController player);
		protected virtual void OnPlayerTouch() { }
		protected virtual void OnPlayerLeave() { }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	/// <summary>
	/// 剧情场景玩家可交互物体的基类
	/// </summary>
	public abstract class PlayerInteractable : MonoBehaviour {

		[SerializeField] private GameObject _outline;

		protected virtual void Awake() {
			Leave();
		}

		/// <summary>
		/// 与该物体交互
		/// </summary>
		/// <param name="player">与该物体交互的玩家</param>
		public void Interact(StoryPlayerController player) => OnInteract(player);

		/// <summary>
		/// 玩家靠近该物体
		/// </summary>
		public void Touch() {
			if (_outline != null) _outline.SetActive(true);
			OnPlayerTouch();
		}

		/// <summary>
		/// 玩家离开该物体
		/// </summary>
		public void Leave() {
			if (_outline != null) _outline.SetActive(false);
			OnPlayerLeave();
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

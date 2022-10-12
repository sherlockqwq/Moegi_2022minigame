using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	public abstract class PlayerInteractable : MonoBehaviour {

		[SerializeField] private GameObject _outline;

		protected virtual void Awake() {
			Leave();
		}

		public void Interact(StoryPlayerController player) => OnInteract(player);
		public void Touch() {
			if (_outline != null) _outline.SetActive(true);
			OnPlayerTouch();
		}
		public void Leave() {
			if (_outline != null) _outline.SetActive(false);
			OnPlayerLeave();
		}
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

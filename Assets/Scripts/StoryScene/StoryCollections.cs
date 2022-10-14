using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene {

	/// <summary>
	/// 剧情关卡中需要收集的物品
	/// </summary>
	public class StoryCollections : PlayerInteractable {
		/// <summary>
		/// 该物体是否已被收集？
		/// </summary>
		public bool Collected { get; private set; }

		[SerializeField] private SpriteRenderer _sprite;

		protected override void OnInteract(StoryPlayerController player) => CollectCoroutine().ApplyTo(this);
		IEnumerator CollectCoroutine() {
			SetActive(false);

			StoryPlayerController.Current.Pause(out var id);

			yield return EasyTools.Gradient.Linear(0.5f, d => {
				if (_sprite != null) _sprite.SetA(1 - d);
				if (_outline != null) _outline.SetA(1 - d);
			});

			Collected = true;
			StoryPlayerController.Current.Resume(id);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;

namespace StoryScene {

	/// <summary>
	/// 收集完毕后的传送门
	/// </summary>
	public class StoryScenePortal : PlayerInteractable {
		[SerializeField] private GameObject collectScene, repairScene;
		[SerializeField] private StoryCollections[] allCollections;

		private void Start() => Waiting().ApplyTo(this);

		IEnumerator Waiting() {
			repairScene.SetActive(false);
			SetActive(false);
			GetComponent<SpriteRenderer>().enabled = false;

			// 等待所有物品都被收集
			yield return Wait.Until(() => allCollections.All(item => item.Collected));

			SetActive(true);
			GetComponent<SpriteRenderer>().enabled = true;
		}

		// 切换时该物体将被 Disable，故使用 TransitionManager 挂载协程
		protected override void OnInteract(StoryPlayerController player) => ToRepairScene().ApplyTo(TransitionManager.Current);

		IEnumerator ToRepairScene() {
			StoryPlayerController.Current.Pause(out var id);
			yield return TransitionManager.Current.ShowMaskCoroutine(Color.white);  // 白屏淡入

			collectScene.SetActive(false);
			repairScene.SetActive(true);    // 切换场景

			yield return TransitionManager.Current.HideMaskCoroutine(Color.white);  // 白屏淡出
			StoryPlayerController.Current.Resume(id);
		}
	}
}

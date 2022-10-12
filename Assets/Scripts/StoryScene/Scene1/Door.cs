using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene.Scene1 {

	public class Door : PlayerInteractable {
		[SerializeField] private Cinemachine.CinemachineVirtualCamera _leftCam, _rightCam;
		[SerializeField] private Transform _leftPos, _rightPos;

		protected override void OnInteract(StoryPlayerController player) => ChangePlayerPos(player).ApplyTo(this);

		IEnumerator ChangePlayerPos(StoryPlayerController player) {
			StoryPlayerController.Current.Pause(out var id);
			yield return TransitionManager.Current.ShowMaskCoroutine();

			if (player.transform.position.x < transform.position.x) {
				player.transform.position = _rightPos.position;
				_rightCam.Priority = 20;
				_leftCam.Priority = 10;
			}
			else {
				player.transform.position = _leftPos.position;
				_leftCam.Priority = 20;
				_rightCam.Priority = 10;
			}

			yield return TransitionManager.Current.HideMaskCoroutine();
			StoryPlayerController.Current.Resume(id);
		}
	}
}

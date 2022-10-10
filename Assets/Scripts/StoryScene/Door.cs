using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	public class Door : MonoBehaviour, IPlayerInteractable {
		public static bool IsFading { get; private set; } = false;

		[SerializeField] private Cinemachine.CinemachineVirtualCamera _leftCam, _rightCam;
		[SerializeField] private Transform _leftPos, _rightPos;
		void IPlayerInteractable.Interact(StoryPlayerController player) {
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
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene {

	public class Door : MonoBehaviour, IPlayerInteractable {
		public static bool IsFading { get; private set; } = false;

		[SerializeField] private Cinemachine.CinemachineVirtualCamera _leftCam, _rightCam;
		[SerializeField] private Transform _leftPos, _rightPos;
		[SerializeField] private GameObject _outline;

		void Awake() {
			_outline.SetActive(false);
		}

		void IPlayerInteractable.OnInteract(StoryPlayerController player) => ChangePlayerPos(player).ApplyTo(this);

		IEnumerator ChangePlayerPos(StoryPlayerController player) {
			IsFading = true;
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
			IsFading = false;
		}

		void IPlayerInteractable.OnPlayerTouch(StoryPlayerController player) => _outline.SetActive(true);

		void IPlayerInteractable.OnPlayerLeave(StoryPlayerController player) => _outline.SetActive(false);
	}
}

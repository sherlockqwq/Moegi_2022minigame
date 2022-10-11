using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	public interface IPlayerInteractable {
		public void OnInteract(StoryPlayerController player);
		public void OnPlayerTouch(StoryPlayerController player);
		public void OnPlayerLeave(StoryPlayerController player);
	}
}

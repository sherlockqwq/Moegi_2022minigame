using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	public interface IPlayerInteractable {
		public void Interact(StoryPlayerController player);
	}
}

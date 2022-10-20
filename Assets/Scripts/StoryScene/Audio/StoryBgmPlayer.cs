using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryScene {

	public class StoryBgmPlayer : MonoBehaviour {
		[SerializeField] private AudioClip _bgm;
		void Awake() => StoryAudio.FadeBGM(_bgm);
	}
}

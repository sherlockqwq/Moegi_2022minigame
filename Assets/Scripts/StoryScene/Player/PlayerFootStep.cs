using UnityEngine;

namespace StoryScene {

	[RequireComponent(typeof(AudioSource))]
	public class PlayerFootStep : MonoBehaviour {
		private AudioSource _source;
		void Awake() {
			_source = GetComponent<AudioSource>();
		}

		public void PlayFootStep() {
			_source.pitch = Random.Range(0.8f, 1.2f);
			_source.Play();
		}
	}
}

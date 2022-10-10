using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using NaughtyAttributes;

namespace EasyTools {

	public class VideoPlayerPreview : MonoBehaviour {
		[Button("播放")]
		private void Play() {
			GetComponent<VideoPlayer>()?.Play();
		}

		[Button("暂停")]
		private void Pause() {
			GetComponent<VideoPlayer>()?.Pause();
		}

		[Button("停止")]
		private void Stop() {
			GetComponent<VideoPlayer>()?.Stop();
		}
	}
}

using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace EasyTools {

	public class VideoPlayerPreview : MonoBehaviour {
		internal void Play() {
			GetComponent<VideoPlayer>()?.Play();
		}

		internal void Pause() {
			GetComponent<VideoPlayer>()?.Pause();
		}

		internal void Stop() {
			GetComponent<VideoPlayer>()?.Stop();
		}
	}

#if UNITY_EDITOR

	[CustomEditor(typeof(VideoPlayerPreview))]
	public class VideoPlayerPreviewEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			var t = target as VideoPlayerPreview;
			if (GUILayout.Button("播放")) t.Play();
			else if (GUILayout.Button("暂停")) t.Pause();
			else if (GUILayout.Button("停止")) t.Stop();
		}
	}

#endif
}

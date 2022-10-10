using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;

namespace StoryScene {

	/// <summary>
	/// 剧情场景中的玩家控制器（暂时不需要刚体？）
	/// </summary>
	public class StoryPlayerController : MonoBehaviour {
		[SerializeField] private KeyCode _leftKey = KeyCode.A, _rightKey = KeyCode.D;
		[SerializeField] private KeyCode _interactKey = KeyCode.E, _deleteKey = KeyCode.Q;

		public static StoryPlayerController Current { get; private set; }

		void Awake() {
			Current = this;
			ResumeAll();
		}

		#region 暂停相关

		public static bool Controlling { get; private set; } = true;
		private static int _controlIndex = 0;
		private static HashSet<int> _pauseList = new HashSet<int>();
		public static void Pause(out int pauseId) {
			Controlling = false;
			pauseId = ++_controlIndex;
			_pauseList.Add(_controlIndex);
		}
		public static void Resume(int pauseId) {
			_pauseList.Remove(pauseId);
			Controlling = _pauseList.Count == 0;
		}
		public static void ResumeAll() {
			_pauseList.Clear();
			Controlling = true;
		}

		#endregion
	}
}

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
		[SerializeField] private float _speed = 1f;

		public static StoryPlayerController Current { get; private set; }

		void Awake() {
			Current = this;
		}

		void Update() {
			if (!Paused) {
				Move();
			}
		}

		#region 移动

		private void Move() {
			var dir = new Vector2((Input.GetKey(_leftKey) ? -1 : 0) + (Input.GetKey(_rightKey) ? 1 : 0), 0);
			transform.Translate(dir * _speed * Time.deltaTime);
		}

		#endregion

		#region 暂停相关

		public bool Paused => DialogManager.Showing;

		#endregion
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using NaughtyAttributes;

namespace StoryScene {

	[RequireComponent(typeof(Rigidbody2D))]
	public class StoryPlayerController : MonoBehaviour {
		[SerializeField] private KeyCode _leftKey = KeyCode.A, _rightKey = KeyCode.D;
		[SerializeField] private KeyCode _interactKey = KeyCode.E, _deleteKey = KeyCode.Q;
		[SerializeField] private float _speed = 1f;

		[Header("触发器")]
		[SerializeField] private int _triggerBufferSize = 10;
		[SerializeField] private LayerMask _triggerLayer;
		private Collider2D[] _triggerBuffer;

		public static StoryPlayerController Current { get; private set; }

		private Rigidbody2D _rb2d;
		private Collider2D _collider2d;

		void Awake() {
			Current = this;
			_rb2d = GetComponent<Rigidbody2D>();
			_collider2d = GetComponent<Collider2D>();
			_triggerBuffer = new Collider2D[_triggerBufferSize];
		}

		void Update() {
			if (!Paused) {
				Move();
				CheckInteractable();
			}
			else Stop();
			if (Input.GetKeyDown(KeyCode.Minus)) transform.Translate(-5, 0, 0);
			if (Input.GetKeyDown(KeyCode.Equals)) transform.Translate(5, 0, 0);
		}

		#region 移动

		private void Move() {
			var dir = new Vector2((Input.GetKey(_leftKey) ? -1 : 0) + (Input.GetKey(_rightKey) ? 1 : 0), 0);
			_rb2d.velocity = dir * _speed;
		}

		private void Stop() {
			_rb2d.velocity = Vector2.zero;
		}

		#endregion

		#region 交互

		private void CheckInteractable() {
			var count = Physics2D.OverlapPointNonAlloc(_rb2d.position, _triggerBuffer, _triggerLayer);
			IPlayerInteractable interactable = null;
			for (int i = 0; i < count; i++) {
				if (_triggerBuffer[i].TryGetComponent<IPlayerInteractable>(out interactable)) {
					// TODO 显示交互图标
					if (Input.GetKeyDown(_interactKey)) {
						interactable.Interact(this);
					}
					break;
				}
			}
			if (interactable == null) {
				// TODO 隐藏交互图标
			}
		}

		#endregion

		#region 暂停相关

		public bool Paused => DialogManager.Showing || Door.IsFading;

		#endregion

	}
}

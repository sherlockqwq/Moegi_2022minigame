using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using NaughtyAttributes;

namespace StoryScene {

	[RequireComponent(typeof(Rigidbody2D))]
	public class StoryPlayerController : MonoBehaviour {
		[Header("移动")]
		[SerializeField] private float _speed = 1f;
		[SerializeField] private KeyCode _leftKey = KeyCode.A, _rightKey = KeyCode.D;

		[Header("交互")]
		[SerializeField] private GameObject _interactTip;
		[SerializeField] private KeyCode _interactKey = KeyCode.E, _deleteKey = KeyCode.Q;

		[Header("触发器")]
		[SerializeField] private int _triggerBufferSize = 10;
		[SerializeField] private LayerMask _triggerLayer;
		private Collider2D[] _triggerBuffer;


		public static StoryPlayerController Current { get; private set; }
		public static StoryPlayerModel Model { get; private set; }

		private Rigidbody2D _rb2d;
		private Collider2D _collider2d;

		void Awake() {
			Current = this;
			Model = GetComponentInChildren<StoryPlayerModel>();
			_rb2d = GetComponent<Rigidbody2D>();
			_collider2d = GetComponent<Collider2D>();
			_triggerBuffer = new Collider2D[_triggerBufferSize];
		}

		void Update() {
			if (!IsPaused) {
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
			Model.SetAnimVelocity(_rb2d.velocity.x);
		}

		private void Stop() {
			_rb2d.velocity = Vector2.zero;
			Model.SetAnimVelocity(0);
		}

		#endregion

		#region 交互

		private PlayerInteractable _lastInteractable;

		/// <summary>
		/// 与可交互物体的触碰与交互的相关逻辑
		/// </summary>
		private void CheckInteractable() {
			var count = Physics2D.OverlapPointNonAlloc(_rb2d.position, _triggerBuffer, _triggerLayer);
			PlayerInteractable interactable = null;
			for (int i = 0; i < count; i++) {
				if (_triggerBuffer[i].TryGetComponent<PlayerInteractable>(out interactable)) { // 有可交互物体
					_interactTip.SetActive(true);   // 显示提示图标
					if (Input.GetKeyDown(_interactKey)) interactable.Interact(this);  // 检查交互按键键
					break;  // 只和该物体交互
				}
			}
			if (interactable == null) _interactTip.SetActive(false);  // 若当前不可交互则隐藏提示图标

			if (_lastInteractable != interactable) {    // 可交互物体发生变动
				_lastInteractable?.Leave();
				interactable?.Touch();
				_lastInteractable = interactable;
			}
		}

		#endregion

		#region 暂停相关

		public bool IsPaused { get; private set; }

		private int _controlIndex = 0;
		private HashSet<int> _pauseList = new HashSet<int>();

		public void Pause(out int pauseId) {
			IsPaused = true;
			pauseId = ++_controlIndex;
			_pauseList.Add(_controlIndex);
		}
		public void Resume(int pauseId) {
			_pauseList.Remove(pauseId);
			IsPaused = _pauseList.Count != 0;
		}
		public void ResumeAll() {
			_pauseList.Clear();
			IsPaused = false;
		}

		#endregion

	}
}

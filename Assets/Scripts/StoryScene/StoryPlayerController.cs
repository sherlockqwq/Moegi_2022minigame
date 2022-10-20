using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using NaughtyAttributes;

namespace StoryScene {

	/// <summary>
	/// 剧情场景中的玩家
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class StoryPlayerController : MonoBehaviour {
		[Header("移动")]
		[SerializeField] private float _speed = 1f;
		[SerializeField] private KeyCode _leftKey = KeyCode.A, _rightKey = KeyCode.D;

		[Header("交互")]
		[SerializeField] private GameObject _interactTip;
		[SerializeField] private KeyCode _interactKey = KeyCode.E, _deleteKey = KeyCode.Q;
		[SerializeField] private AudioClip _keySound;

		[Header("飘字")]
		[SerializeField] private TMPro.TMP_Text _floating;
		[SerializeField] private AudioClip _floatingSound;

		[Header("触发器")]
		[SerializeField] private int _triggerBufferSize = 10;
		[SerializeField] private LayerMask _triggerLayer;
		private Collider2D[] _triggerBuffer;


		public static StoryPlayerController Current { get; private set; }
		public static StoryPlayerModel Model { get; private set; }

		private Rigidbody2D _rb2d;

		void Awake() {
			Current = this;
			Model = GetComponentInChildren<StoryPlayerModel>();
			_rb2d = GetComponent<Rigidbody2D>();
			_triggerBuffer = new Collider2D[_triggerBufferSize];

			FloatingAwake();
		}

		void Update() {
			if (!IsPaused) {
				Move();
				CheckInteractable();
			}
			else {
				Stop();
				ClearInteractTip();
			}
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

		private PlayerInteractable _lastInteractable, _currentInteractable;

		/// <summary>
		/// 与可交互物体的触碰与交互的相关逻辑
		/// </summary>
		private void CheckInteractable() {
			bool deleteDown = Input.GetKeyDown(_deleteKey), interactDown = Input.GetKeyDown(_interactKey);

			if (deleteDown || interactDown) StoryAudio.PlaySFX(_keySound);

			_currentInteractable = null;

			int count = Physics2D.OverlapPointNonAlloc(_rb2d.position, _triggerBuffer, _triggerLayer);
			for (int i = 0; i < count; i++) {
				if (_triggerBuffer[i].TryGetComponent<PlayerInteractable>(out _currentInteractable)) { // 有可交互物体
					break;  // 只和该物体交互
				}
			}

			if (_currentInteractable == null) {  // 没有可交互物体
				_interactTip.SetActive(false);  // 则隐藏提示图标
			}
			else {
				if (_currentInteractable.Replaceable) {  // 是要替换的物体
					if (deleteDown) _currentInteractable.Interact(this); // 检查按键
				}
				else {  // 不是要替换的物体
					_interactTip.SetActive(true);   // 显示提示图标
					if (interactDown) _currentInteractable.Interact(this);   // 检查交互按键
				}
			}

			if (_lastInteractable != _currentInteractable) { // 可交互物体发生变动
				_lastInteractable?.Leave();
				_currentInteractable?.Touch();
				_lastInteractable = _currentInteractable;
			}

		}

		private void ClearInteractTip() {
			_lastInteractable?.Leave();
			_lastInteractable = null;
			_interactTip.SetActive(false);
		}

		#endregion

		#region 暂停相关

		public static bool IsPaused { get; private set; }

		private static int _controlIndex = 0;
		private static HashSet<int> _pauseList = new HashSet<int>();

		/// <summary>
		/// 暂停当前玩家
		/// </summary>
		/// <param name="pauseId">用于恢复玩家控制的ID</param>
		public static void Pause(out int pauseId) {
			IsPaused = true;
			pauseId = ++_controlIndex;
			_pauseList.Add(_controlIndex);
		}
		/// <summary>
		/// 恢复玩家控制
		/// </summary>
		/// <param name="pauseId">通过 Pause 函数获取的ID</param>
		public static void Resume(int pauseId) {
			_pauseList.Remove(pauseId);
			IsPaused = _pauseList.Count != 0;
		}
		/// <summary>
		/// 完全恢复玩家控制（无视当前暂停的数量）
		/// </summary>
		public static void ResumeAll() {
			_pauseList.Clear();
			IsPaused = false;
		}

		#endregion

		#region 飘字

		private Vector3 _floatingPos;
		private Coroutine _floatPosCoroutine;

		private void FloatingAwake() {
			_floatingPos = _floating.transform.localPosition;
		}

		public IEnumerator ShowFloating() {
			if (_floatPosCoroutine != null) StopCoroutine(_floatPosCoroutine);
			_floating.transform.localPosition = _floatingPos;
			_floatPosCoroutine = EasyTools.Gradient.Linear(
				3f, _ => _floating.transform.Translate(Vector3.up * 0.1f * Time.deltaTime)
			).ApplyTo(this);

			StoryAudio.PlaySFX(_floatingSound);

			yield return EasyTools.Gradient.Linear(0.5f, _floating.SetA);
			yield return Wait.Seconds(1.5f);
			yield return EasyTools.Gradient.Linear(0.5f, d => _floating.SetA(1 - d));
		}

		#endregion
	}
}

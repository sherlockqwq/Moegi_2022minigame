using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyTools {

	public class SimpleAnimation : MonoBehaviour {
		[SerializeField] private Vector3 _moveSpeed, _rotateSpeed;
		[SerializeField] private Sprite[] _sprites;
		[SerializeField] private float _frameRate;

		IEnumerator Start() {
			if (_sprites.Length > 0 && TryGetComponent<SpriteRenderer>(out var sr)) {
				float interval = 1 / _frameRate;
				int index = 0;
				while (true) {
					sr.sprite = _sprites.ChooseLoop(index++);
					yield return Wait.Seconds(interval);
				}
			}
		}

		void Update() {
			transform.Translate(_moveSpeed * Time.deltaTime);
			transform.Rotate(_rotateSpeed * Time.deltaTime);
		}
	}
}

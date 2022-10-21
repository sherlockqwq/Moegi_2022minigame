using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace StoryScene {

	/// <summary>
	/// 剧情场景中玩家的模型
	/// </summary>
	public class StoryPlayerModel : MonoBehaviour {
		[Header("动画")]
		[SerializeField] private Animator _anim;
		[SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Bool)] private int _walking;

		/// <summary>
		/// 设置玩家的当前速度（左负右正）
		/// </summary>
		public void SetAnimVelocity(float velocity) {
			if (velocity < -0.01f) {
				FaceLeft(true);
				_anim.SetBool(_walking, true);
			}
			else if (velocity > 0.01f) {
				FaceLeft(false);
				_anim.SetBool(_walking, true);
			}
			else _anim.SetBool(_walking, false);
		}

		/// <summary>
		/// 设置面向左边还是右边
		/// </summary>
		public void FaceLeft(bool faceLeft) => transform.localScale = new Vector3(faceLeft ? -1 : 1, 1, 1);
	}
}

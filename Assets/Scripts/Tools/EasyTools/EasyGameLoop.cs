using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EasyTools {

	/// <summary>
	/// 游戏运行时始终不被摧毁的 MonoBehaviour
	/// </summary>
	public class EasyGameLoop : MonoBehaviour {
		private static EasyGameLoop _instance;
		private static EasyGameLoop Instance {
			get {
				if (_instance == null) {
					var obj = new GameObject("EasyGameLoop");
					DontDestroyOnLoad(obj);
					_instance = obj.AddComponent<EasyGameLoop>();
				}
				return _instance;
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Startup() => Instance.enabled = true;

		/// <summary>
		/// 挂载协程
		/// </summary>
		public static Coroutine Do(IEnumerator routine) => Instance.StartCoroutine(routine);
		/// <summary>
		/// 停止协程
		/// </summary>
		public static void Stop(Coroutine routine) {
			if (routine != null) Instance.StopCoroutine(routine);
		}
	}
}

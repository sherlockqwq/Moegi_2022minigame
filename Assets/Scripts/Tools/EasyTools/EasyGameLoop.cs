using UnityEngine;

namespace EasyTools {

	public class EasyGameLoop : MonoBehaviour {
		private static EasyGameLoop _instance;
		public static EasyGameLoop Instance {
			get {
				if (_instance == null) {
					var obj = new GameObject("EasyGameLoop");
					DontDestroyOnLoad(obj);
					_instance = obj.AddComponent<EasyGameLoop>();
				}
				return _instance;
			}
		}
	}
}

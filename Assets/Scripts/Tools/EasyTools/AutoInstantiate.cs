using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace EasyTools {

	public class AutoInstantiate : ScriptableObject {
		private static string _fileName = "AutoInstantiateSettings";

		public static bool IsStartup { get; private set; } = false;
		public static bool IsSceneLoaded { get; private set; } = false;

		[SerializeField] private List<GameObject> _dontDestroyOnLoad = new List<GameObject>();
		[SerializeField] private List<GameObject> _instantiateOnSceneLoaded = new List<GameObject>();

		private static AutoInstantiate _current;

		[RuntimeInitializeOnLoadMethod]
		private static void StartupLoad() {
			SceneManager.sceneLoaded += OnSceneLoaded;
			_current = Resources.Load<AutoInstantiate>(_fileName);

			IsStartup = true;
			if (_current) {
				foreach (var item in _current._dontDestroyOnLoad) {
					if (item != null) DontDestroyOnLoad(Instantiate(item));
				}
			}
			IsStartup = false;
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			IsSceneLoaded = true;
			if (_current) {
				foreach (var item in _current._instantiateOnSceneLoaded) {
					if (item != null) Instantiate(item);
				}
			}
			IsSceneLoaded = false;
		}

#if UNITY_EDITOR

		[MenuItem("EasyTools/AutoInstantiate/Create Settings")]
		private static void CreateSettings() {
			var _fullName = _fileName + ".asset";
			var dir = Path.Combine(Application.dataPath, "Resources");

			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			if (!File.Exists(Path.Combine(dir, _fullName))) {
				AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AutoInstantiate>(), $"Assets/Resources/{_fullName}");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

#endif
	}
}

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace EasyTools {

	public static class EasyLocalization {
		public const string DefaultLang = "zh-CN";
		public static string CurrentLang { get; set; } = DefaultLang;

		private static Dictionary<string, Dictionary<string, object>> _lib;

		public static event Action onLangSwitched = delegate { };

		public static bool SwitchLang(string lang) {
			if (SetLang(lang)) {
				onLangSwitched();
				return true;
			}
			else return false;
		}
		private static bool SetLang(string lang) {
			var path = Application.streamingAssetsPath + $"/EasyTools/Localization/{lang}";
			if (!Directory.Exists(path)) return false;
			CurrentLang = lang;
			var allFiles = Directory.GetFiles(path, "*.json");
			if (_lib == null) _lib = new Dictionary<string, Dictionary<string, object>>();
			else _lib.Clear();
			foreach (var fileName in allFiles) {
				_lib[Path.GetFileNameWithoutExtension(fileName)] = FromJson<Dictionary<string, object>>(File.ReadAllText(fileName));
			}
			return true;
		}

		public static T Get<T>(string fileName, string key) {
			if (_lib == null) ResetDefaultLang();
			if (_lib.TryGetValue(fileName, out var dict) && dict.TryGetValue(key, out var value)) return value.To<T>();
			else return default;
		}

		public static bool TryGet<T>(string fileName, string key, out T value) {
			if (_lib == null) ResetDefaultLang();
			if (_lib.TryGetValue(fileName, out var dict) && dict.TryGetValue(key, out var v)) {
				value = v.To<T>();
				return true;
			}
			else {
				value = default;
				return false;
			}
		}

		public static T GetTutorial<T>(string key) => Get<T>("Tutorial", key);
		public static T GetSleep<T>(string key) => Get<T>("Sleep", key);
		public static T GetFloat<T>(string key) => Get<T>("Float", key);
		public static T GetBook<T>(string key) => Get<T>("Book", key);
		public static T GetDailyDialog<T>(string key) => Get<T>("DailyDialog", key);
		public static T GetWindow<T>(string key) => Get<T>("Window", key);
		public static T GetInteractable<T>(string key) => Get<T>("InteractableMessage", key);
		public static T GetDream<T>(string key) => Get<T>("Dream", key);


		private static T To<T>(this object obj) => FromJson<T>(ToJson(obj));
		private static string ToJson(object obj) => JsonConvert.SerializeObject(obj);
		private static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);

#if UNITY_EDITOR
		[UnityEditor.MenuItem("EasyTools/Localization/Reset Default Lang")]
#endif
		public static void ResetDefaultLang() {
			SetLang(DefaultLang);
		}
	}
}

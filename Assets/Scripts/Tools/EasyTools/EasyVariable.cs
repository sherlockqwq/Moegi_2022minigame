using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace EasyTools {

	public static class EasyVariable {
		private static Dictionary<string, object> dict;

		public static bool Reload() {
			var path = Application.streamingAssetsPath + $"/EasyTools/Variables.json";
			if (!File.Exists(path)) {
				Debug.LogError("EasyTools/Variables.json文件不存在");
				return false;
			}
			// TODO 异步读取
			var json = File.ReadAllText(path);
			dict = FromJson<Dictionary<string, object>>(json);
			return true;
		}

		public static T Get<T>(string key) {
			if (dict == null && !Reload()) return default;
			else if (dict.TryGetValue(key, out var data)) return data.To<T>();
			else return default;
		}

		private static T To<T>(this object obj) => FromJson<T>(ToJson(obj));
		private static string ToJson(object obj) => JsonConvert.SerializeObject(obj);
		private static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
	}
}

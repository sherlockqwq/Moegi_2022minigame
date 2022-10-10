using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace EasyTools {

	public static class EasySave {
		private struct SaveLoadAction { public Action saveAction, loadAction; }
		private static int id = 0;
		private static Dictionary<int, object> data = new Dictionary<int, object>();
		private static Dictionary<int, SaveLoadAction> actions = new Dictionary<int, SaveLoadAction>();

		private static string directory = "EasySave";
		private static string tempFileName = "EasySaveTemp";
		private static string IndexFileName(int index) => $"EasySave{index}";
		private static string SaveDir => Path.Combine(Application.persistentDataPath, directory);
		private static string GetFilePath(string fileName) => Path.Combine(Application.persistentDataPath, directory, fileName);

		public static bool DoClear { get; set; } = true;
		public static bool Exists() => Exists(tempFileName);
		public static bool Exists(int index) => Exists(IndexFileName(index));
		public static bool Exists(string fileName) => File.Exists(GetFilePath(fileName));

		public static void Save() => SaveTo(tempFileName);
		public static void SaveTo(int index) => SaveTo(IndexFileName(index));
		public static void SaveTo(string fileName) {
			if (DoClear) data.Clear();
			actions.Values.Each(a => a.saveAction());
			Directory.CreateDirectory(SaveDir);
			// TODO 异步保存
			File.WriteAllText(GetFilePath(fileName), ToJson(data));
		}

		public static void Load() => LoadFrom(tempFileName);
		public static void LoadFrom(int index) => LoadFrom(IndexFileName(index));
		public static void LoadFrom(string fileName) {
			if (DoClear) data.Clear();
			if (!Exists(fileName)) {
				Debug.LogError($"试图读取不存在的存档文件：{fileName}");
				return;
			}
			// TODO 异步读取
			var json = File.ReadAllText(GetFilePath(fileName));
			data = FromJson<Dictionary<int, object>>(json);
			actions.Values.Each(a => a.loadAction());
		}

		public static int Register<T>(Func<T> saveData, Action<T> loadData) => Register<T>(saveData, loadData, null);
		public static int Register<T>(Func<T> saveData, Action<T> loadData, Action onLoadFailed) {
			var key = id++;
			actions.Add(key, new SaveLoadAction() {
				saveAction = () => { data[key] = saveData(); },
				loadAction = () => {
					if (data.ContainsKey(key)) loadData(data[key].To<T>());
					else onLoadFailed?.Invoke();
				}
			});
			return key;
		}
		public static int Register<T>(Func<T> saveData, Action<T> loadData, T defaultValue) {
			var key = id++;
			actions.Add(key, new SaveLoadAction() {
				saveAction = () => { data[key] = saveData(); },
				loadAction = () => {
					if (data.ContainsKey(key)) loadData(data[key].To<T>());
					else loadData(defaultValue);
				}
			});
			return key;
		}
		public static bool Unregister(int id) => actions.Remove(id);

		private static T To<T>(this object obj) => FromJson<T>(ToJson(obj));
		private static string ToJson(object obj) => JsonConvert.SerializeObject(obj);
		private static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);

#if UNITY_EDITOR
		[UnityEditor.MenuItem("EasyTools/EasySave/Open Directory")]
		private static void OpenDir() {
			Directory.CreateDirectory(SaveDir);
			System.Diagnostics.Process.Start(SaveDir);
		}
#endif
	}
}

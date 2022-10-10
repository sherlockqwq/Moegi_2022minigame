using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyTools {

	[ExecuteAlways]
	public class EasyLocalizationText : MonoBehaviour {
		[HideInInspector] public Object target;
		[HideInInspector] public string propertyName = "text", fileName = "", key = "";

		private void Reset() {
			if (TryGetComponent<Text>(out var t)) target = t;
			else if (TryGetComponent<TextMesh>(out var tm)) target = tm;
			else if (TryGetComponent<TMP_Text>(out var tmp)) target = tmp;

			Refresh();
			EasyLocalization.onLangSwitched -= Refresh;
			EasyLocalization.onLangSwitched += Refresh;
		}

		private void Start() {
			if (Application.IsPlaying(gameObject)) {
				Refresh();
				EasyLocalization.onLangSwitched -= Refresh;
				EasyLocalization.onLangSwitched += Refresh;
			}
		}

		private void OnDestroy() {
			EasyLocalization.onLangSwitched -= Refresh;
		}

		public void Refresh() {
			if (target != null && !string.IsNullOrWhiteSpace(propertyName) && EasyLocalization.TryGet<string>(fileName, key, out var s)) {
				target.GetType().GetProperty(propertyName)?.SetValue(target, s);
				target.GetType().GetField(propertyName)?.SetValue(target, s);
			}
		}
	}

#if UNITY_EDITOR

	[CanEditMultipleObjects]
	[CustomEditor(typeof(EasyLocalizationText))]
	public class EasyLocalizationTextEditor : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			serializedObject.Update();
			var t = target as EasyLocalizationText;
			var targetProperty = serializedObject.FindProperty("target");

			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(targetProperty, new GUIContent(""));
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				t.Refresh();
			}

			var options = new List<string>();
			int idx = 0;

			options.Clear();
			idx = -1;
			if (targetProperty.objectReferenceValue != null) {
				var fields = targetProperty.objectReferenceValue.GetType().GetFields();
				foreach (var f in fields) {
					if (f.FieldType == typeof(string)) {
						if (f.Name == t.propertyName) idx = options.Count;
						options.Add(f.Name);
					}
				}
				var props = targetProperty.objectReferenceValue.GetType().GetProperties();
				foreach (var f in props) {
					if (f.PropertyType == typeof(string) && f.CanWrite) {
						if (f.Name == t.propertyName) idx = options.Count;
						options.Add(f.Name);
					}
				}
			}

			EditorGUI.BeginChangeCheck();
			idx = EditorGUILayout.Popup(idx, options.ToArray());
			if (EditorGUI.EndChangeCheck() || idx == -1) {
				serializedObject.FindProperty("propertyName").stringValue = idx == -1 ? null : options[idx];
				serializedObject.ApplyModifiedProperties();
				t.Refresh();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("fileName"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				t.Refresh();
			}

			if (GUILayout.Button("刷新")) {
				t.Refresh();
			}
		}
	}

#endif
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyTools {

	[RequireComponent(typeof(AudioSource))]
	public class AudioController : MonoBehaviour {
		public AudioSource Source => _audio;
		private AudioSource _audio;
		public bool IsStarted { get; private set; } = false;
		public bool IsPaused { get; private set; } = false;
		public float ProgressTime => _audio.time;
		public float Length => _audio.clip.length;

		private void Awake() {
			_audio = GetComponent<AudioSource>();
			_audio.playOnAwake = false;
			_audio.Stop();
		}

		Coroutine _timer;
		public void Play(Action onFinished = null) {
			IsStarted = true;
			IsPaused = false;

			if (_audio.time == 0) _audio.Play();
			else _audio.UnPause();

			_timer = this.Delay(Length - ProgressTime, () => {
				Stop();
				onFinished?.Invoke();
			});
		}

		public void Pause() {
			if (IsStarted && !IsPaused) {
				IsPaused = true;
				StopCoroutine(_timer);
				_audio.Pause();
			}
		}
		public void Stop() {
			IsStarted = false;
			IsPaused = false;
			StopCoroutine(_timer);
			_audio.Stop();
			_audio.time = 0;
		}
	}
}

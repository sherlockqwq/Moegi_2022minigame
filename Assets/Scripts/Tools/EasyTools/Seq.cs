using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyTools {

	public class SeqBase {
		private class SeqItem {
			public bool isCoroutine;
			public System.Action action;
			public IEnumerator coroutine;
		}

		private List<SeqItem> _seq = new List<SeqItem>();

		protected SeqBase Add(System.Action action) {
			_seq.Add(new SeqItem() { isCoroutine = false, action = action });
			return this;
		}
		protected SeqBase Add(IEnumerator coroutine) {
			_seq.Add(new SeqItem() { isCoroutine = true, coroutine = coroutine });
			return this;
		}

		protected IEnumerator ToCoroutine() {
			foreach (var item in _seq) {
				if (item.isCoroutine) yield return item.coroutine;
				else item.action();
			}
		}
		protected Coroutine ApplyTo(MonoBehaviour mono) => mono.StartCoroutine(ToCoroutine());
		protected Coroutine Apply() => EasyGameLoop.Do(ToCoroutine());
	}


	public class Seq : SeqBase {
		new public Seq Add(System.Action action) => (Seq)base.Add(action);
		new public Seq Add(IEnumerator coroutine) => (Seq)base.Add(coroutine);
		new public IEnumerator ToCoroutine() => base.ToCoroutine();
		new public Coroutine ApplyTo(MonoBehaviour mono) => base.ApplyTo(mono);
		new public Coroutine Apply() => base.Apply();
	}
}

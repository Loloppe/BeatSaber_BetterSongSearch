﻿using HMUI;
using IPA.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BetterSongSearch.Util {
	static class BSMLStuff {
		public static void UnleakTable(GameObject gameObject) {
			foreach(var x in gameObject.GetComponentsInChildren<Touchable>(true).Where(x => !x.gameObject.activeSelf)) {
				var l = x.gameObject;

				GameObject.DestroyImmediate(x);
				GameObject.DestroyImmediate(l.GetComponent<CanvasRenderer>());
				GameObject.DestroyImmediate(l);
			}
		}

		public static GameObject GetScrollbarForTable(GameObject table, Transform targetContainer) {
			var scrollBar = Resources.FindObjectsOfTypeAll<VerticalScrollIndicator>().FirstOrDefault()?.transform.parent?.gameObject;

			if(scrollBar == null)
				return null;

			var sw = table.GetComponentInChildren<ScrollView>();

			if(sw == null)
				return null;

			var listScrollBar = GameObject.Instantiate(scrollBar, targetContainer, false);

			ReflectionUtil.SetField(sw, "_verticalScrollIndicator", listScrollBar.GetComponentInChildren<VerticalScrollIndicator>());

			var buttoneZ = listScrollBar.GetComponentsInChildren<NoTransitionsButton>().OrderByDescending(x => x.gameObject.name == "UpButton").ToArray();
			if(buttoneZ.Length == 2) {
				ReflectionUtil.SetField(sw, "_pageUpButton", (Button)buttoneZ[0]);
				ReflectionUtil.SetField(sw, "_pageDownButton", (Button)buttoneZ[1]);

				buttoneZ[0].onClick.AddListener(sw.PageUpButtonPressed);
				buttoneZ[1].onClick.AddListener(sw.PageDownButtonPressed);
			}

			// I dont know WHY I need do do this, but if I dont the scrollbar wont work with the added modal.
			foreach(Transform x in listScrollBar.transform) {
				foreach(var y in x.GetComponents<Behaviour>())
					y.enabled = true;
			}

			sw.Update();

			return scrollBar;
		}
	}
}

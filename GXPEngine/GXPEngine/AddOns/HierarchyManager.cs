using GXPEngine;
using System.Collections.Generic;

namespace GXPEngine {
	struct GameObjectPair {
		public GameObject parent;
		public GameObject child;
		public GameObjectPair(GameObject par, GameObject ch) {
			parent = par;
			child = ch;
		}
	}

	/// <summary>
	/// If you are getting strange bugs because you are calling AddChild or Destroy during the Update loop, 
	/// you can use this class to do this more cleanly: when using 
	/// HierarchyManager.Instance.LateAdd or HierarchyManager.Instance.LateDestroy,
	/// all these hierarchy changes will be made after the update loop is finished.
	/// Similarly, you can use HierarchyManager.Instance.LateCall to postpone a certain method call until 
	/// after the update loop.
	/// </summary>
	class HierarchyManager {
		public delegate void DelayedMethod();

		public static HierarchyManager Instance {
			get {
				if (instance == null) {
					instance = new HierarchyManager ();
				}				
				return instance;
			}
		}
		private static HierarchyManager instance; 

		private List<GameObject> toDestroy;
		private List<GameObjectPair> toAdd;
		private List<DelayedMethod> toCall;

		// Don't construct these yourself - get the one HierarchyManager using HierarchyManager.Instance
		HierarchyManager() {
			Game.main.OnAfterStep += UpdateHierarchy;
			toDestroy = new List<GameObject> ();
			toAdd = new List<GameObjectPair> ();
			toCall = new List<DelayedMethod> ();
		}

		/*
		public void Destroy() {
			Game.main.OnAfterStep -= UpdateHierarchy;
			instance = null;
		}
		*/

		public void LateDestroy(GameObject obj) {
			toDestroy.Add (obj);
		}

		public bool IsOnDestroyList(GameObject obj) {
			return toDestroy.Contains (obj);
		}

		public void LateAdd(GameObject parent, GameObject child) {
			toAdd.Add (new GameObjectPair (parent, child));
		}

		public void LateCall(DelayedMethod meth) {
			toCall.Add (meth);
		}

		public void UpdateHierarchy() {
			// First add, then destroy, to prevent memory leaks (e.g. adding a projectile in a level that's about to be destroyed!)
			foreach (GameObjectPair pair in toAdd) {
				pair.parent.AddChild (pair.child);
			}
			toAdd.Clear ();

			foreach (GameObject obj in toDestroy) {
				obj.Destroy ();
			}
			toDestroy.Clear ();

			foreach (DelayedMethod method in toCall) {
				method ();
			}
			toCall.Clear ();
		}
	}
}

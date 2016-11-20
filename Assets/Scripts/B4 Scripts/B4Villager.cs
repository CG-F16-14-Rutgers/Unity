using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4Villager : MonoBehaviour {
	protected int treeType;

	private BehaviorAgent behaviorAgent;
	void Start () {
		this.treeType = UnityEngine.Random.Range (0, 100);
		if (treeType >= 0 && treeType <= 33) {
			behaviorAgent = new BehaviorAgent (this.BuildTreeType1 ());
		} else if (treeType > 33 && treeType <= 66) {
			behaviorAgent = new BehaviorAgent (this.BuildTreeType2 ());
		} else if (treeType > 66 && treeType <= 100) {
			behaviorAgent = new BehaviorAgent (this.BuildTreeType3 ());
		}
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	protected Node BuildTreeType1() {
		return null;
	}

	protected Node BuildTreeType2() {
		return null;
	}

	protected Node BuildTreeType3() {
		return null;
	}
}
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4Villager : MonoBehaviour {
	protected int treeType;
	public GameObject[] villagers;

	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;

	private BehaviorAgent behaviorAgent;
	void Start () {
		for (int i = 0; i < villagers.Length; i++) { 
			this.treeType = UnityEngine.Random.Range (0, 100);
			if (treeType >= 0 && treeType <= 33) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType1 (i));
			} else if (treeType > 33 && treeType <= 66) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType2 (i));
			} else if (treeType > 66 && treeType <= 100) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType3 (i));
			}
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}
	}

	protected Node ST_Approach(Transform target, int villagerIndex)
	{
		Vector3 position = new Vector3(target.position.x+(UnityEngine.Random.Range(-3,3)),0,target.position.z+(UnityEngine.Random.Range(-3,3)));
		return new Sequence(this.villagers[villagerIndex].GetComponent<BehaviorMecanim>().Node_GoTo(position));
	}

	protected Node BuildTreeType1(int villagerIndex) {
		Node path = new Sequence (this.ST_Approach(positionA, villagerIndex),this.ST_Approach(positionB, villagerIndex));
		Node root = new DecoratorLoop (path);
		return root;
	}

	protected Node BuildTreeType2(int villagerIndex) {
		Node path = new Sequence (this.ST_Approach(positionB, villagerIndex),this.ST_Approach(positionA, villagerIndex));
		Node root = new DecoratorLoop (path);
		return root;
	}

	protected Node BuildTreeType3(int villagerIndex) {
		Node path = new Sequence (this.ST_Approach(positionC, villagerIndex),this.ST_Approach(positionB, villagerIndex));
		Node root = new DecoratorLoop (path);
		return root;
	}
}
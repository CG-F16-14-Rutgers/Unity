using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4Villager : MonoBehaviour {
	protected int treeType;
	public GameObject[] villagers;
	private Hashtable types;

	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;
	public Transform positionE;
	public Transform positionF;
	public Transform positionG;
	public Transform positionH;

	private BehaviorAgent behaviorAgent;
	void Start () {
		types = new Hashtable ();
		for (int i = 0; i < villagers.Length; i++) { 
			this.treeType = UnityEngine.Random.Range (0, 100);
			if (treeType >= 0 && treeType <= 33) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType1 (i));
				types.Add (i, 1);
			} else if (treeType > 33 && treeType <= 66) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType2 (i));
				types.Add (i, 2);
			} else if (treeType > 66 && treeType <= 100) {
				behaviorAgent = new BehaviorAgent (this.BuildTreeType3 (i));
				types.Add (i, 3);
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

	protected int checkForPeople(int villagerIndex) {
		for (int index = 0; index < villagers.Length; index++) {
			if ((index != villagerIndex) && (Vector3.Distance (villagers [index].transform.position, villagers[villagerIndex].transform.position) < 2.0f)) {
				//if(types[index] != types[villagerIndex]) 
					return index;
			}
		}
		return villagerIndex;
	}

	protected RunStatus changeDirection(Vector3 newDirection, int villagerIndex) {
		villagers [villagerIndex].transform.forward = newDirection;
		return RunStatus.Success;
	}

	protected RunStatus callOverAnim (int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("CallOver");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("CallOver");
		return RunStatus.Success;
	}

	protected RunStatus cheerTogetherAnim(int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("Cheer");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("Cheer");
		return RunStatus.Success;
	}

	protected RunStatus danceAnim(int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("BD1");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("BD1");
		return RunStatus.Success;
	}

	protected RunStatus chestSaluteAnim(int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("ChestPumpSalute");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("ChestPumpSalute");
		return RunStatus.Success;
	}

	protected RunStatus handsUpAnim(int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("HandsUp");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("HandsUp");
		return RunStatus.Success;
	}

	protected RunStatus hitFromBehindAnim(int villagerIndex, int otherVilIndex) {
		villagers [villagerIndex].GetComponent<Animator> ().Play ("HitFromBehind");
		villagers [otherVilIndex].GetComponent<Animator> ().Play ("HitFromBehind");
		return RunStatus.Success;
	}

	protected Node BuildTreeType1(int villagerIndex) {
		Val<int> nearbyPerson = Val.V(() => checkForPeople(villagerIndex));
		Func<RunStatus> changeFacingCurrent = () => changeDirection (villagers [nearbyPerson.Value].transform.position, villagerIndex);
		Func<RunStatus> changeFacingOther = () => changeDirection (villagers [villagerIndex].transform.position, nearbyPerson.Value);

		Func<RunStatus> callOver = () => callOverAnim (villagerIndex,nearbyPerson.Value);
		Node callOverNode = new Sequence(new LeafInvoke(callOver), new LeafWait(4000));

		Func<RunStatus> cheerTogether = () => cheerTogetherAnim (villagerIndex, nearbyPerson.Value);
		Node cheerTogetherNode = new Sequence(new LeafInvoke(cheerTogether), new LeafWait(5000));

		Node randomActions = new SelectorShuffle (callOverNode,cheerTogetherNode);
		Node path = new SequenceShuffle (randomActions, new SelectorShuffle(this.ST_Approach(positionA, villagerIndex),this.ST_Approach(positionB, villagerIndex),this.ST_Approach(positionC, villagerIndex), this.ST_Approach(positionD, villagerIndex), this.ST_Approach(positionE, villagerIndex), this.ST_Approach(positionF, villagerIndex), this.ST_Approach(positionG, villagerIndex),this.ST_Approach(positionH, villagerIndex)));
		Node root = new DecoratorLoop (path);
		return root;
	}

	protected Node BuildTreeType2(int villagerIndex) {
		Val<int> nearbyPerson = Val.V(() => checkForPeople(villagerIndex));
		Func<RunStatus> changeFacingCurrent = () => changeDirection (villagers [nearbyPerson.Value].transform.position, villagerIndex);
		Func<RunStatus> changeFacingOther = () => changeDirection (villagers [villagerIndex].transform.position, nearbyPerson.Value);

		Func<RunStatus> dance = () => danceAnim (villagerIndex,nearbyPerson.Value);
		Node danceNode = new Sequence(new LeafInvoke(dance), new LeafWait(10000));

		Func<RunStatus> chestSalute = () => chestSaluteAnim (villagerIndex, nearbyPerson.Value);
		Node chestSaluteNode = new Sequence(new LeafInvoke(chestSalute), new LeafWait(5000));

		Node randomActions = new SelectorShuffle (danceNode,chestSaluteNode);
		Node path = new SequenceShuffle (randomActions, new SelectorShuffle(this.ST_Approach(positionA, villagerIndex),this.ST_Approach(positionB, villagerIndex),this.ST_Approach(positionC, villagerIndex), this.ST_Approach(positionD, villagerIndex), this.ST_Approach(positionE, villagerIndex), this.ST_Approach(positionF, villagerIndex), this.ST_Approach(positionG, villagerIndex),this.ST_Approach(positionH, villagerIndex)));
		Node root = new DecoratorLoop (path);
		return root;
	}

	protected Node BuildTreeType3(int villagerIndex) {
		Val<int> nearbyPerson = Val.V(() => checkForPeople(villagerIndex));
		Func<RunStatus> changeFacingCurrent = () => changeDirection (villagers [nearbyPerson.Value].transform.position, villagerIndex);
		Func<RunStatus> changeFacingOther = () => changeDirection (villagers [villagerIndex].transform.position, nearbyPerson.Value);

		Func<RunStatus> handsUp = () => handsUpAnim (villagerIndex,nearbyPerson.Value);
		Node handsUpNode = new Sequence(new LeafInvoke(handsUp), new LeafWait(4000));

		Func<RunStatus> hitFromBehind = () => hitFromBehindAnim (villagerIndex, nearbyPerson.Value);
		Node hitFromBehindNode = new Sequence(new LeafInvoke(hitFromBehind), new LeafWait(5000));

		Node randomActions = new SelectorShuffle (handsUpNode,hitFromBehindNode);
		Node path = new SequenceShuffle (randomActions, new SelectorShuffle(this.ST_Approach(positionA, villagerIndex),this.ST_Approach(positionB, villagerIndex),this.ST_Approach(positionC, villagerIndex), this.ST_Approach(positionD, villagerIndex), this.ST_Approach(positionE, villagerIndex), this.ST_Approach(positionF, villagerIndex), this.ST_Approach(positionG, villagerIndex),this.ST_Approach(positionH, villagerIndex)));
		Node root = new DecoratorLoop (path);
		return root;
	}
}
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4BehaviorCop : MonoBehaviour
{
	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;
	public GameObject cop;
	public GameObject friend;
	public GameObject victim;

	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	bool checkForCriminals () {
		if (Vector3.Distance (cop.transform.position, friend.transform.position) < 7.0f) {
			return true;
		} else {
			return false;
		}
	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( cop.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node BuildTreeRoot()
	{
		Val<bool> criminalInRange = Val.V (() => this.checkForCriminals());
		Func<bool> act1 = () => (criminalInRange.Value == false);
		Node lookoutNode = new DecoratorLoop(new LeafAssert (act1));
		Node patrolNode = new DecoratorLoop (
			new SequenceShuffle(
				this.ST_ApproachAndWait(this.positionA),
				this.ST_ApproachAndWait(this.positionB),
				this.ST_ApproachAndWait(this.positionC),
				this.ST_ApproachAndWait(this.positionD)));

		Node phasePatrol = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(lookoutNode, patrolNode)));

		Val<UnityEngine.Transform> criminalPosition = Val.V (() => this.friend.transform);
		Func<bool> act2 = () => (criminalInRange.Value == true);
		Node chaseNode = new DecoratorLoop(new LeafAssert (act2));
		Node followCriminal = new DecoratorLoop (new Sequence (this.ST_ApproachAndWait (criminalPosition.Value)));

		Node phaseChase = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel (chaseNode, followCriminal)));

		Node root = new DecoratorLoop (new SelectorParallel (phasePatrol, phaseChase));

		return root;
	}
}

using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorVictim : MonoBehaviour
{
	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
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

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( victim.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node BuildTreeBeginning()
	{
		Val<bool> status = Val.V (() => BehaviorManager.Instance.beginning);
		Func<bool> act = () => (status.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		Node path = new DecoratorLoop (
			new SequenceShuffle(
				this.ST_ApproachAndWait(this.positionA),
				new LeafWait(1000),
				this.ST_ApproachAndWait(this.positionB)));
		return new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(phaseNode, path));
	}

	protected Node BuildTreeMiddleAndEnd()
	{
		Val<bool> statusMid = Val.V (() => BehaviorManager.Instance.middle);
		Val<bool> statusEnd = Val.V (() => BehaviorManager.Instance.end);
		Func<bool> act = () => (statusMid.Value == true || statusEnd.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		Node path = new DecoratorLoop (
			new SequenceShuffle(
				this.ST_ApproachAndWait(this.positionC)));
		return new DecoratorForceStatus (RunStatus.Success, new SequenceParallel (phaseNode, path));
	}

	protected Node BuildTreeRoot()
	{
		return new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel (this.BuildTreeBeginning (), this.BuildTreeMiddleAndEnd ())));
	}
}

using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorCop : MonoBehaviour
{
	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;
	public GameObject cop;

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
		return new Sequence( cop.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node BuildTreeRoot()
	{
		Node roaming = new DecoratorLoop (
			new SequenceShuffle(
				this.ST_ApproachAndWait(this.positionA),
				this.ST_ApproachAndWait(this.positionB),
				this.ST_ApproachAndWait(this.positionC),
				this.ST_ApproachAndWait(this.positionD)));
		return roaming;
	}
}

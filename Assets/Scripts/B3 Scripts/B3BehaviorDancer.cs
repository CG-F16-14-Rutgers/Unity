using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorDancer : MonoBehaviour
{
	public GameObject dancer;
	public GameObject friend;

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

	protected RunStatus beginAndEndDance()
	{
		BehaviorManager.Instance.changeDance ();
		print (BehaviorManager.Instance.dance);
		return RunStatus.Success;
	}

	protected Node BuildTreeRoot()
	{
		print(Vector3.Distance(dancer.transform.position, friend.transform.position));
		Val<bool> status = Val.V (() => BehaviorManager.Instance.middle);
		Func<bool> act = () => (status.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		Val<Vector3> dancerPos = Val.V (() => this.dancer.transform.position);
		Val<Vector3> friendPos = Val.V (() => this.friend.transform.position);
		Func<bool> trigger = () => (Vector3.Distance(dancerPos.Value, friendPos.Value) < 5.0f);
		Node triggerNode = new DecoratorLoop(new LeafAssert (trigger));

		Func<RunStatus> changeDance = () => (this.beginAndEndDance());
		Node changeDanceNode = new LeafInvoke (changeDance);

		Val<string> danceName = "BREAKDANCE";

		Node danceNode = new DecoratorLoop (
			new Sequence(
				changeDanceNode,
				new LeafWait(1000),
				this.dancer.GetComponent<BehaviorMecanim>().Node_BodyAnimation(danceName, true),
				new LeafWait(10000),
				this.dancer.GetComponent<BehaviorMecanim>().Node_BodyAnimation(danceName, false),
				new LeafWait(1000),
				this.dancer.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true),
				new LeafWait(3000),
				changeDanceNode));

		print ("Gets here");

		Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel (phaseNode, triggerNode, danceNode)));
		return root;
	}
}
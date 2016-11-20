using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorInformant : MonoBehaviour
{
	public GameObject friend;
	public GameObject informant;

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

	protected RunStatus beginStory() {
		print ("phase changed");
		BehaviorManager.Instance.changeBeginning ();
		return RunStatus.Success;
	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => new Vector3(target.position.x-1, 0, target.position.z));
		return new Sequence( informant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node BuildTreeRoot() {
		Val<bool> status = Val.V (() => BehaviorManager.Instance.beginning);
		Func<bool> act = () => (status.Value == false);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		Node wonderful = new Sequence (informant.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WONDERFUL", true), new LeafWait (2000),informant.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WONDERFUL", false));

		Func<RunStatus> beginStory = () => (this.beginStory());
		Node begin = new LeafInvoke (beginStory);
		Node firstPhase = new Sequence (this.ST_ApproachAndWait (friend.transform),wonderful, begin);
		Node root = new DecoratorLoop (new DecoratorForceStatus(RunStatus.Success, new SequenceParallel (phaseNode, firstPhase)));
		return root;
	}

}
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorFriend : MonoBehaviour
{
	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;

	public GameObject cop;

	private Animator animator;

	protected bool pickedUpKey = false;
	protected bool alive = true;
	//private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start ()
	{
		/*behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();*/
		this.animator = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (alive) {
			if (Vector3.Distance (cop.transform.position, this.transform.position) < 1.0f) {
				alive = false;
				this.animator.Play ("Dying");
			}
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.RightArrow)) { 
				float horizontalArrow = Input.GetAxis ("Horizontal");
				float verticalArrow = Input.GetAxis ("Vertical");
				Vector3 rotation = new Vector3 (0, horizontalArrow * 3, 0);
				this.transform.Rotate (rotation);

				Vector3 inputMovement = new Vector3 (0.0f, 0.0f, verticalArrow);
				this.transform.Translate (inputMovement * Time.deltaTime * 7);
				this.animator.SetFloat ("Speed", 1);
			} else {
				this.animator.SetFloat ("Speed", 0);
			}
			if (BehaviorManager.Instance.beginning == true) {
				if (pickedUpKey == false && (Vector3.Distance (this.transform.position, this.positionB.transform.position) < 1.0f)) {
					this.animator.Play ("Ground_Pickup_Right");
					this.pickedUpKey = true;
				} else if (pickedUpKey == true && (Vector3.Distance (this.transform.position, this.positionC.transform.position) < 1.0f)) {
					this.animator.Play ("Reach");
					BehaviorManager.Instance.beginning = false;
					BehaviorManager.Instance.middle = true;
				}
			} else if (BehaviorManager.Instance.middle == true) {
				if (BehaviorManager.Instance.dance == true) {
					this.animator.Play ("BD1");
				}
			}
		}
	}

	/*protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( friend.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}*/

	/*protected RunStatus endBeginning()
	{
		BehaviorManager.Instance.changeBeginning ();
		BehaviorManager.Instance.changeMiddle ();
		return RunStatus.Success;
	}

	protected RunStatus endMiddle()
	{
		BehaviorManager.Instance.changeMiddle ();
		BehaviorManager.Instance.changeEnd ();
		return RunStatus.Success;
	}

	protected RunStatus disableDoor()
	{
		Destroy (GameObject.FindGameObjectWithTag ("door"));
		return RunStatus.Success;
	}


	/*protected Node BuildTreeBeginning()
	{
		//Needs some way to change the boolean beginning in BehaviorManager when beginning phase has ended
		Val<bool> status = Val.V (() => BehaviorManager.Instance.beginning);
		Func<bool> act = () => (status.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		//Func<bool> changeStatus = () => (BehaviorManager.Instance.beginning = false);
		Val<string> reachAnimationName = "REACH";

		Func<RunStatus> endBegin = () => (this.endBeginning());
		Node phaseChangeNode = new LeafInvoke (endBegin);

		Func<RunStatus> doorDisabler = () => (this.disableDoor());
		Node doorDisablerNode = new LeafInvoke (doorDisabler);

		Node path = new DecoratorLoop (
			new Sequence (
				this.ST_ApproachAndWait (this.positionA),
				this.ST_ApproachAndWait (this.positionB),
				friend.GetComponent<BehaviorMecanim> ().Node_HandAnimation (reachAnimationName, true),
				new LeafWait(10000),
				friend.GetComponent<BehaviorMecanim> ().Node_HandAnimation (reachAnimationName, false),
				this.ST_ApproachAndWait (this.positionC),
				friend.GetComponent<BehaviorMecanim> ().Node_HandAnimation (reachAnimationName, true),
				new LeafWait(10000),
				doorDisablerNode,
				friend.GetComponent<BehaviorMecanim> ().Node_HandAnimation (reachAnimationName, false),
				phaseChangeNode));

		Node beginning = new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(phaseNode, path));
		return beginning;
	}

	protected Node BuildTreeMiddle()
	{
		//Need to change middle bool when conditions met
		Val<bool> status = Val.V (() => BehaviorManager.Instance.middle);
		/*Func<bool> act = () => (status.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));*//*

		Val<bool> danceStatus = Val.V (() => BehaviorManager.Instance.dance);
		Func<bool> checkDanceTrue = () => (danceStatus.Value == true && status.Value == true);
		Node danceTrueAssertNode = new DecoratorLoop (new LeafAssert (checkDanceTrue));

		Val<string> danceAnimationName = "BREAKDANCE";

		Node dance = new DecoratorLoop(
			new Sequence(
				friend.GetComponent<BehaviorMecanim> ().Node_BodyAnimation (danceAnimationName, true), 
				new LeafWait(5000),
				friend.GetComponent<BehaviorMecanim> ().Node_BodyAnimation (danceAnimationName, false),
				new LeafWait(5000)));
		Node danceBattle = /*new DecoratorForceStatus (RunStatus.Success,*//* new SequenceParallel (danceTrueAssertNode, dance);

		Func<RunStatus> endMiddle = () => (this.endMiddle());
		Node phaseChangeNode = new LeafInvoke (endMiddle);

		Func<bool> checkDanceFalse = () => (danceStatus.Value == false && status.Value == true);
		Node danceFalseAssertNode = new DecoratorLoop (new LeafAssert(checkDanceFalse));
		Node path = new DecoratorLoop (
			new Sequence (
				this.ST_ApproachAndWait (this.positionD),
				phaseChangeNode));
		Node escape = /*new DecoratorForceStatus(RunStatus.Success,*//* new SequenceParallel (danceFalseAssertNode, path);

		Node middle = new DecoratorForceStatus(RunStatus.Success, new SelectorParallel (danceBattle, escape));
		return middle;
	}

	protected Node BuildTreeEnd()
	{
		Val<bool> status = Val.V (() => BehaviorManager.Instance.end);
		Func<bool> act = () => (status.Value == true);
		Node phaseNode = new DecoratorLoop(new LeafAssert (act));

		Val<string> endAnimationName = "BREAKDANCE";
		Node endAnimationNode = new Sequence (
			friend.GetComponent<BehaviorMecanim> ().Node_BodyAnimation (endAnimationName, true),
			new LeafWait (10000),
			friend.GetComponent<BehaviorMecanim> ().Node_BodyAnimation (endAnimationName, false));

		Node end = new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(phaseNode, endAnimationNode));
		return end;
	}

	protected Node BuildTreeRoot()
	{
		Node root = new DecoratorLoop (new DecoratorForceStatus(RunStatus.Success, new SequenceParallel (this.BuildTreeBeginning (), this.BuildTreeMiddle (), this.BuildTreeEnd())));
		return root;
	}*/
}
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorInformant : MonoBehaviour
{
	public GameObject friend;
	public GameObject informant;

	private bool isMoving = false;
	private Animator animator;
	// Use this for initialization
	void Start ()
	{
		this.animator = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update ()
	{
		float horizontalArrow = Input.GetAxis ("Horizontal");
		float verticalArrow = Input.GetAxis ("Vertical");
		Vector3 inputMovement = new Vector3 (horizontalArrow, 0.0f, verticalArrow);
		this.transform.position += inputMovement * Time.deltaTime * 5.0f;
		if (horizontalArrow != 0 || verticalArrow != 0 && isMoving == false) {
			this.animator.Play ("WalkRun");
			this.isMoving = true;
		} else {
			this.animator.Play ("Idle");
			this.isMoving = false;
		}
		if (Vector3.Distance (this.informant.transform.position, this.friend.transform.position) < 2.0f) {
			BehaviorManager.Instance.beginning = true;
		}
	}

}
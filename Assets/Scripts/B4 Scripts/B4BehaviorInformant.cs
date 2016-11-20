using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4BehaviorInformant : MonoBehaviour
{
	public GameObject friend;

	private Animator animator;
	// Use this for initialization
	void Start ()
	{
		this.animator = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)) { 
			float horizontalArrow = Input.GetAxis ("Horizontal");
			float verticalArrow = Input.GetAxis ("Vertical");
			Vector3 rotation = new Vector3 (0, horizontalArrow*3, 0);
			this.transform.Rotate (rotation);

			Vector3 inputMovement = new Vector3 (0.0f, 0.0f, verticalArrow);
			this.transform.Translate (inputMovement*Time.deltaTime*7);
			this.animator.SetFloat ("Speed", 1);
		} else {
			this.animator.SetFloat ("Speed", 0);
		}
		if (Vector3.Distance (this.transform.position, this.friend.transform.position) < 2.0f) {
			BehaviorManager.Instance.beginning = true;
		}
	}

}
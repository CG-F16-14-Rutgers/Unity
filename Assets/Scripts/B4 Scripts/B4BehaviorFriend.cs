using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B4BehaviorFriend : MonoBehaviour
{
	public Transform positionA;
	public Transform positionB;
	public Transform positionC;
	public Transform positionD;

	public GameObject cop;

	private Animator animator;

	protected bool pickedUpKey = false;
	protected bool alive = true;

	// Use this for initialization
	void Start ()
	{
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
					Destroy(GameObject.FindGameObjectWithTag ("door"));
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
}
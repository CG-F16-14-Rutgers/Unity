using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class B3BehaviorInformant : MonoBehaviour
{
	public GameObject friend;
	public GameObject informant;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		float horizontalArrow = Input.GetAxis ("Horizontal");
		float verticalArrow = Input.GetAxis ("Vertical");
		Vector3 inputMovement = new Vector3 (horizontalArrow, 0.0f, verticalArrow);
		this.transform.position += inputMovement * Time.deltaTime * 15.0f;
		if (Vector3.Distance (this.informant.transform.position, this.friend.transform.position) < 2.0f) {
			BehaviorManager.Instance.beginning = true;
		}
	}

}
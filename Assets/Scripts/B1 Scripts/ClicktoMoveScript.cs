using UnityEngine;
using System.Collections;


public class ClicktoMoveScript : MonoBehaviour {

	private Animator anim;
	private NavMeshAgent navMeshAgent;
	private Renderer matRender;
	private Color origColor;
	private Color highlightColor;
	private bool isSelected;
	private bool isWalking;
	private bool isRunning;
	private bool isJumping;
	public DirectorController director;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		matRender = GetComponent<Renderer> ();
		origColor = matRender.material.color;
		highlightColor = Color.white;
		isSelected = false;
	}

	// Update is called once per frame
	void Update () {
		if (isSelected) {
			if (Input.GetMouseButtonDown (0)) {
				if (director.beginBrakes == true) {
					director.beginBrakes = false;
					director.stoppedAgents.Clear ();
				}
				navMeshAgent.Resume ();
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
					navMeshAgent.destination = hit.point;
					isWalking = true;
					navMeshAgent.updatePosition = true;
					navMeshAgent.updateRotation = true;
					navMeshAgent.nextPosition = transform.position;
					anim.SetBool ("isWalking", isWalking);
				}
			} else if (Vector3.Distance(transform.position,navMeshAgent.destination) < 1.0f) {
				if (director.beginBrakes != true) {
					navMeshAgent.Stop ();
					director.beginBrakes = true;
					director.stoppedAgents = new Hashtable ();
					director.stoppedAgents.Add (gameObject.name, gameObject);
				}
			} 
		}
	}

	void OnMouseEnter () {
		matRender.material.color = highlightColor;
	}

	void OnMouseExit () {
		if(!isSelected) 
			matRender.material.color = origColor;
	}

	void OnMouseDown () {
		if (!isSelected) {
			isSelected = true;
		} else {
			isSelected = false;
		}
	}
}
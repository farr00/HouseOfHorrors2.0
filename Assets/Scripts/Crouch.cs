using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
	CharacterController characterCollider;
	// Use this for initialization
	void Start ()
	{
		characterCollider = gameObject.GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.LeftControl)) {
			characterCollider.height = 1.0f;
			//Mathf.SmoothDamp
		} else if (!Physics.SphereCast(new Ray(transform.position,Vector3.up),characterCollider.radius,characterCollider.height+0.7f)){
			characterCollider.height = 1.8f;
		}
	}
}
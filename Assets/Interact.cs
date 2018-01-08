using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
	public Vector3 com;
	public Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = com;



	}

	void Update ()
	{
		float y = Mathf.Clamp (rb.rotation.eulerAngles.y, -100, 270);
		rb.rotation = Quaternion.Euler (rb.rotation.eulerAngles.x, y, rb.rotation.eulerAngles.z);
	
	}


}


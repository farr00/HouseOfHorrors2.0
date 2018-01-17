﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

	// Declare Variables

	private Animator anim;
	public GameObject player;
	//public List<GameObject> hidespots = new List<GameObject>();
	private Transform target;
	private NavMeshAgent agent;

	public float walkSpeed = 10f;
	public float runSpeed = 5f;
	public float damageRange = 2f;
	public float wanderRange;
	private AudioSource growl;

	float touchedTime = 0;

	string currentState = "idle";
	bool attacking = false;

	Vector3 goTo;
	bool inSight;
	bool alive = true;
	bool finding = false;
	Vector3 randomDirection;

	UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;

	// Make a easy function to change the zombies state

	public void SetState(string state){
		if (currentState != state) {
			currentState = state;
			if (state == "idle") {
				agent.speed = 0;
			} else if (state == "running" && runSpeed > 1) {
				agent.speed = runSpeed;
			} else if (state == "walking" || (state == "running" && runSpeed < 1)) {
				agent.speed = walkSpeed;
				currentState = "walking";
			}
			if (attacking == false) {
				anim.SetTrigger ("is" + currentState.Substring (0, 1).ToUpper () + currentState.Substring (1, currentState.Length-1));
			}
		}
	}

	// When the game begings load these components and set variables

	void Start(){
		//print (hidespots);
		goTo = transform.position;
		playerController = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ();
		growl = gameObject.GetComponent<AudioSource> ();
		growl.Play ();
		target = player.transform;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
//		if (hidespots.Count == 0) {
//			foreach (GameObject hidespot in GameObject.FindGameObjectsWithTag ("hspot")) {
//				hidespots.Add (hidespot);
//			}
		}

	// Attack the player when this is called

	private IEnumerator Attack(){
		anim = GetComponent<Animator> ();
		bool killed = false;
		yield return new WaitForSeconds (.1f);
		if ((target.position - transform.position).magnitude < damageRange) { // Double check the range before attacking
			Cutscene cutscene = player.GetComponent<Cutscene>();
			cutscene.Attacked ();
			currentState = "idle";
			killed = true;
			alive = false;
			agent.speed = 0;
		}

		yield return new WaitForSeconds (2.03f);
		if (killed) {
			anim.SetTrigger ("isIdle");
			SetState ("idle");
		}else{
			anim.SetTrigger ("is" + currentState.Substring (0, 1).ToUpper () + currentState.Substring (1, currentState.Length-1)); // If it didnt kill return to old anim
		}

		attacking = false;
<<<<<<< HEAD
		CapsuleCollider capCollider = GetComponent<CapsuleCollider> ();
		float rad = capCollider.radius;
		capCollider.isTrigger = false;
		capCollider.radius = 0;
		yield return new WaitForSeconds (.5f);
		capCollider.radius = rad;
		capCollider.isTrigger = true;
=======
>>>>>>> 8b1626ad1753f3fef36a96d6bf8c509c2555bf96
	}

	// When theres a collision run this

	void OnTriggerEnter(Collider c){
		RaycastHit hit;
		int layerMask = 1 << LayerMask.NameToLayer ("Monster");
		if (c.gameObject.CompareTag ("Player") && Time.time > touchedTime && alive && Physics.Linecast (transform.position, player.transform.position, out hit, ~layerMask)) {
			print (hit.transform);
		

<<<<<<< HEAD
			if (hit.transform == player.transform) {
=======
			if (hit.transform.CompareTag("Player")) {
>>>>>>> 8b1626ad1753f3fef36a96d6bf8c509c2555bf96
				print ("OK PLZ RUN");
				attacking = true;
				transform.LookAt (target);
				touchedTime = Time.time + 2.63f;
				anim.SetTrigger (playerController.IsUnderDesk ? "isAttacking" : "isAttacking");
<<<<<<< HEAD
				anim.SetTrigger ("isAttacking");
=======
>>>>>>> 8b1626ad1753f3fef36a96d6bf8c509c2555bf96
				StartCoroutine (Attack ());
			}// Check tag to see if it can attack
			
		}
	}

	// Every frame do this

	void FixedUpdate(){
		//if (desks.Count == 0) {
		//	desks = GameObject.FindGameObjectsWithTag ("Desk");
		//}
		RaycastHit hit;
		int layerMask = 1 << LayerMask.NameToLayer("Monster");
		if (Physics.Linecast(transform.position, player.transform.position, out hit, ~layerMask)){
			//print (hit.transform);
<<<<<<< HEAD
			if (hit.transform == player.transform && alive && !(playerController.IsUnderDesk && !inSight)) {
				goTo = player.transform.position;
				inSight = true;
			}else if(inSight && hit.transform != player.transform)  {
=======
			if (hit.transform.CompareTag("Player") && alive && !(playerController.IsUnderDesk && !inSight)) {
				goTo = player.transform.position;
				inSight = true;
			}else if(inSight && !hit.transform.CompareTag("Player"))  {
>>>>>>> 8b1626ad1753f3fef36a96d6bf8c509c2555bf96
				inSight = false;
				goTo = player.transform.position;

			}
			if (currentState == "walking" && inSight) {
				agent.SetDestination (goTo);

			}
		}
	}

	void Update(){
		print (Vector3.Distance(player.transform.position,goTo));
		if (finding) {
			randomDirection = Random.insideUnitSphere * wanderRange;
			randomDirection += player.transform.position;
			NavMeshHit hit;
			if (NavMesh.SamplePosition (randomDirection, out hit, 20, 1)) {
				finding = false;
				goTo = hit.position;
			}
		}

			if (Vector3.Distance (player.transform.position, goTo) > wanderRange) {
				finding = true;
			}
		
		if (currentState != "idle") {
			agent.SetDestination (goTo);// Move to player if the state is not idle
		}
		if (((agent.remainingDistance  <= agent.stoppingDistance && inSight == false) || alive == false) && !attacking) {
			SetState ("walking");
			if (Vector3.Distance (transform.position, agent.destination) < 1.5f) {
				finding = true;
			}
			agent.speed = walkSpeed;


		}else if (inSight){
			agent.speed = runSpeed;
			SetState ("running");
		}

	}

}
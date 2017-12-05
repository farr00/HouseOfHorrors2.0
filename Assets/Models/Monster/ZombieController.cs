using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

	// Declare Variables

	private Animator anim;
	public GameObject player;
	private Transform target;
	private NavMeshAgent agent;

	public float walkSpeed = 10f;
	public float runSpeed = 5f;
	public float damageRange = 2f;

	private AudioSource growl;

	float touchedTime = 0;

	string currentState = "idle";
	bool attacking = false;

	private Vector3 startPos;

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
		growl = gameObject.GetComponent<AudioSource> ();
		growl.Play ();
		startPos = transform.position;
		target = player.transform;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	// Attack the player when this is called

	private IEnumerator Attack(){
		anim = GetComponent<Animator> ();
		bool killed = false;
		yield return new WaitForSeconds (.5f);
		if ((target.position - transform.position).magnitude < damageRange) { // Double check the range before attacking
			//StartCoroutine(playerController.Attacked ());
			killed = true;
			agent.speed = 0;
		}

		yield return new WaitForSeconds (.5f);
		if (killed) {
			anim.SetTrigger ("isIdle");
			SetState ("idle");
		}else{
			anim.SetTrigger ("is" + currentState.Substring (0, 1).ToUpper () + currentState.Substring (1, currentState.Length-1)); // If it didnt kill return to old anim
		}
		attacking = false;
	}

	// When theres a collision run this

	void OnTriggerEnter(Collider c){
		if (c.gameObject.CompareTag("Player") && Time.time > touchedTime){ // Check tag to see if it can attack
			attacking = true;
			transform.LookAt (target);
			touchedTime = Time.time + 1f;
			anim.SetTrigger ("isAttacking");
			StartCoroutine(Attack ());
		}
	}

	// Every frame do this

	void Update(){
		if (currentState != "idle") {
			agent.SetDestination (target.position); // Move to player if the state is not idle
		}
		SetState ("running");
			
	}

}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Agent agent;

	void Awake() {
		Globals.player = this;
	}

	void Start() {
		agent = GetComponent<Agent>();
	}
	
	void Update() {
		agent.move.MoveDx = Input.GetAxis("Horizontal");
		agent.move.DoJump = Input.GetButton("Jump");
		agent.carry.DoThrow = false;
		agent.carry.DoPickUp = false;
		if(Input.GetButton("Vertical")) {
			if(agent.carry.IsCarrying)
				agent.carry.DoThrow = true;
			else 
				agent.carry.DoPickUp = true;
		}
		agent.slice.DoAttack = Input.GetButton("Fire1");
	}
}

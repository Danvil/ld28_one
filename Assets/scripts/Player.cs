using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Agent agent;
	
	void Awake() {
		agent = GetComponent<Agent>();
	}

	void Start() {
	}
	
	void Update() {
		agent.move.MoveDx = Input.GetAxis("Horizontal");
		agent.move.DoJump = Input.GetButton("Jump");
		agent.move.UseRun = Input.GetKey(KeyCode.LeftShift);
		agent.carry.DoThrow = false;
		agent.carry.DoPickUp = false;
		if(Input.GetButton("Fire3")) {
			if(agent.carry.IsCarrying)
				agent.carry.DoThrow = true;
			else 
				agent.carry.DoPickUp = true;
		}
		agent.slice.DoAttack = Input.GetButton("Fire1");
	}
}

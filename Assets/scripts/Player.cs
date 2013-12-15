using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	AgentMove am;
	AgentCarry ac;
	AgentHealth ah;

	float carryRate = 0.5f;
	float carrySleep = 0.0f;

	void Awake() {
		Globals.player = this;
	}

	void Start() {
		am = GetComponent<AgentMove>();
		ac = GetComponent<AgentCarry>();
		ah = GetComponent<AgentHealth>();
	}
	
	void Update() {
		am.MoveDx = Input.GetAxis("Horizontal");
		am.DoJump = Input.GetButton("Jump");
		ac.DoThrow = false;
		ac.DoPickUp = false;
		carrySleep -= Time.deltaTime;
		if(Input.GetButton("Fire1") && carrySleep < 0) {
			carrySleep = carryRate;
			if(ac.IsCarrying)
				ac.DoThrow = true;
			else 
				ac.DoPickUp = true;
		}
	}
}

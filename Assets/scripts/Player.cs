using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	AgentMove am;

	void Awake() {
		Globals.player = this;
	}

	void Start() {
		am = GetComponent<AgentMove>();
	}
	
	void Update() {
		am.MoveDx = Input.GetAxis("Horizontal");
		am.DoJump = Input.GetButton("Jump");
	}
}

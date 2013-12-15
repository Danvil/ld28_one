using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {

	AgentMove am;
	AgentHealth ah;

	public float desiredX;

	void Start() {
		am = GetComponent<AgentMove>();
		ah = GetComponent<AgentHealth>();
	}
	
	void Update() {
		if(ah.IsDead) return;
		float dx = desiredX - this.transform.position.x;
		am.MoveDx = dx;
	}
}

using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {

	AgentMove am;
	
	public float desiredX;

	void Start() {
		am = GetComponent<AgentMove>();
	}
	
	void Update() {
		float dx = desiredX - this.transform.position.x;
		am.MoveDx = dx;
	}
}

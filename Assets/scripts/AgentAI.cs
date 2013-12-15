using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {

	Agent a;

	public float desiredX;

	void Start() {
		a = GetComponent<Agent>();
	}
	
	void Update() {
		if(a.health.IsDead) return;
		float dx = desiredX - this.transform.position.x;
		a.move.MoveDx = dx;
	}
}

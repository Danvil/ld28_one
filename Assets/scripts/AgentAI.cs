using UnityEngine;
using System.Collections;

public class AgentAI : MonoBehaviour {

	Agent a;

	const float AttackRange = 5.0f;
	const float AfraidPercent = 0.25f;

	public Vector2 Home;

	Vector2 goal;

	void Start() {
		a = GetComponent<Agent>();
		goal = Home;
	}

	bool isAfraid = false;
	bool decidedIfAfraid = false;
	float afraidCooldown = 0.0f;
	
	void Update() {
		if(a.health.IsDead)
			return;
		Vector2 delta = (Globals.player.transform.position - this.transform.position).XY();
		float playerDist = delta.magnitude;
		float playerDistX = Mathf.Abs(delta.x);
		float playerDistY = Mathf.Abs(delta.y);
		// decide if attack
		afraidCooldown -= Time.deltaTime;
		if(afraidCooldown < 0) {
			float dtKilled = Time.time - Globals.PlayerKilledAgentTime;
			if(!decidedIfAfraid && dtKilled < 0.5f) {
				decidedIfAfraid = true;
				isAfraid = (Random.value < AfraidPercent);
				if(isAfraid) {
					afraidCooldown = 3.0f;
				}
			}
			if(dtKilled > 1.5f) {
				decidedIfAfraid = false;
			}
		}
		else {
			isAfraid = false;
		}
		bool isHostile = Globals.IsHostile && !isAfraid;
		// slice
		if(a.HasKnive) {
			// attack
			if(isHostile) {
				if(playerDist < AttackRange) {
					// go to player
					goal = Globals.player.transform.position;
				}
			}
			else {
				goal = Home;
			}
		}
		else {
			// attack
			if(isHostile) {
				if(playerDist < AttackRange) {
					// go to player
					goal = Globals.player.transform.position.XY() + delta.normalized * AttackRange;
				}
			}
			else {
				goal = Home;
			}
		}
		// move
		if(a.move.hasSupport) {
			Vector2 d = goal - this.transform.position.XY();
			a.move.MoveDx = d.x;
		}
		// in range
		if(a.HasKnive && isHostile) {
			if(playerDistX < 0.45f + 0.95f*AgentSlice.ATTACK_RANGE && playerDistY < 0.3f) {
				a.slice.DoAttack = true;
			}
		}
	}
}

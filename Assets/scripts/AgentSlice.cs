﻿using UnityEngine;
using System.Collections;

public class AgentSlice : MonoBehaviour {

	static int ATTACK_LAYER_MASK = (1<<10) + (1<<11);

	public const float ATTACK_RANGE = 0.6f;
	
	Agent agent;

	public float attackRate = 0.6f;
	float nextAttack = 0.0f;
	public GameObject pfSlice;

	public float damage = 3.0f;
	
	public bool DoAttack { get; set; }
	
	void Start() {
		agent = GetComponent<Agent>();
	}
	
	void Update() {
		if(agent.health.IsDead) {
			return;
		}
		nextAttack -= Time.deltaTime;
		if(DoAttack && nextAttack < 0) {
			agent.animator.SetTrigger("slice");
			DoAttack = false;
			nextAttack = attackRate;
			AgentHealth target = FindAttackTarget();
			if(target != null) {
				bool wasDead = target.IsDead;
				target.Health -= damage;
				if(Globals.player.gameObject == this.gameObject && target.IsDead && !wasDead) {
					Globals.PlayerKilledAgentTime = Time.time;
				}
				Vector2 pos = 0.5f*(target.transform.position.XY() + this.transform.position.XY());
				StartCoroutine(Tools.CreateParticleEffect(pfSlice, pos));
			}
		}
	}

	AgentHealth FindAttackTarget() {
		// what direction are we looking at?
		float dir = this.transform.localScale.x;
		// find object
		RaycastHit2D hit = Tools.PickNeighbour((CircleCollider2D)collider2D, transform, dir*Vector3.right, ATTACK_RANGE, ATTACK_LAYER_MASK);
		if(!hit) {
			return null;
		}
		// check if agent
		return hit.rigidbody.gameObject.GetComponent<AgentHealth>();
	}
	
}

using UnityEngine;
using System.Collections;

public class AgentSlice : MonoBehaviour {

	static int ATTACK_LAYER_MASK = (1<<10) + (1<<11);
	
	Agent agent;

	public float attackRate = 0.6f;
	float nextAttack = 0.0f;
	public GameObject pfSlice;
	
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
				target.Health -= 2.0f;
				Vector2 pos = 0.5f*(target.transform.position.XY() + this.transform.position.XY());
				StartCoroutine(CreateSlicePf(pos));
			}
		}
	}

	AgentHealth FindAttackTarget() {
		// what direction are we looking at?
		float dir = this.transform.localScale.x;
		// find object
		RaycastHit2D hit = Tools.PickNeighbour((CircleCollider2D)collider2D, transform, dir*Vector3.right, 0.5f, ATTACK_LAYER_MASK);
		if(!hit) {
			return null;
		}
		// check if agent
		return hit.rigidbody.gameObject.GetComponent<AgentHealth>();
	}
	
	IEnumerator CreateSlicePf(Vector2 pos) {
		GameObject a = (GameObject)Instantiate(pfSlice);
		a.transform.position = pos.XY0() + new Vector3(0,0,-3);
		a.transform.localScale = this.transform.localScale;
		float dur = 0.0f;
		foreach(var ps in a.GetComponentsInChildren<ParticleSystem>()) {
			ps.renderer.sortingLayerName = "Fx";
			dur = Mathf.Max(dur, ps.duration);
		}
		yield return new WaitForSeconds(dur);
		Destroy(a);
	}
	
}

using UnityEngine;
using System.Collections;

public class AgentCarry : MonoBehaviour {

	static int PICK_LAYER_MASK = (1<<8) + (1<<9) + (1<<10);

	public float throwForce = 200.0f;

	Agent agent;
	
	public bool DoPickUp { get; set; }
	public bool DoThrow { get; set; }

	float carryRate = 0.5f;
	float carrySleep = 0.0f;

	GameObject carry;

	public bool IsCarrying {
		get {
			return carry != null;
		}
	}

	void Start() {
		DoPickUp = false;
		DoThrow = false;
		carry = null;
		agent = GetComponent<Agent>();
	}
	
	void Update() {
		if(agent.health.IsDead) {
			if(IsCarrying) {
				PerformThrow(0.0f);
			}
			return;
		}
		carrySleep -= Time.deltaTime;
		if(carrySleep > 0) {
			return;
		}
		if(DoPickUp && !IsCarrying) {
			PerformPickUp();
			carrySleep = carryRate;
			DoPickUp = false;
		}
		if(DoThrow && IsCarrying) {
			PerformThrow(throwForce);
			carrySleep = carryRate;
			DoThrow = false;
		}
	}

	void PerformPickUp() {
		// what direction are we looking at?
		float dir = this.transform.localScale.x;
		// find object
		RaycastHit2D hit = Tools.PickNeighbour((CircleCollider2D)collider2D, transform, dir*Vector3.right, 1.0f, PICK_LAYER_MASK);
		if(!hit) {
			return;
		}
		// connect
		carry = hit.transform.gameObject;
		carry.rigidbody2D.isKinematic = true;
		carry.transform.parent = this.transform;
	}
	
	void PerformThrow(float forceAmount) {
		if(!IsCarrying) {
			return;
		}
		// disconnect
		carry.rigidbody2D.isKinematic = false;
		carry.transform.parent = null;
		// apply velocities
		carry.rigidbody2D.velocity = rigidbody2D.velocity;
		carry.rigidbody2D.angularVelocity = rigidbody2D.angularVelocity;
		// throw force
		float throw_angle = Mathf.Deg2Rad * 35.0f;
		Vector2 throw_dir = this.transform.localScale.XY().CwMult(new Vector2(Mathf.Cos(throw_angle), Mathf.Sin(throw_angle)));
		carry.rigidbody2D.AddForce(forceAmount * throw_dir);
		carry = null;
	}
	
	void FixedUpdate() {
		if(IsCarrying) {
			carry.transform.position = this.transform.position + 1.3f * Vector2.up.XY0();
			// should fix a bug but doesnt
//			float sx = carry.transform.localScale.x;
//			float sy = carry.transform.localScale.y;
//			carry.transform.localScale = new Vector3(Mathf.Sign(sx), Mathf.Sign(sy));
		}
	}
}

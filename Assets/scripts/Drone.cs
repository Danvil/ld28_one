using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour {

	static int moveLayerMask = (1<<8) + (1<<9); // avoid wall and objects

	public float speed = 1.0f;
	public float flyForce = 100.0f;
	public float flySpeedMax = 3.0f;

	public Vector2 Home;

	Vector2 goal;

	Animator animator;
	CircleCollider2D cc2;
	AgentHealth ah;

	void Start() {
		animator = GetComponent<Animator>();
		cc2 = (CircleCollider2D)this.collider2D;
		ah = GetComponent<AgentHealth>();
		goal = Home;
	}

	void SetRandomGoal() {
		goal = Home + 2.7f*Random.insideUnitCircle;
	}

	bool CanMove(Vector2 m) {
		return !Tools.ThreeRayTest2D(cc2, transform, m, 0.1f, 0.5f, moveLayerMask);
	}
	
	void Update() {
		animator.SetBool("dead",  ah.IsDead);
		if(ah.IsDead) return;
		if(needNewGoal) {
			SetRandomGoal();
			needNewGoal = false;
		}
	}

	bool needNewGoal = false;

	void FixedUpdate() {
		if(ah.IsDead) return;
		Vector2 dx = goal - this.transform.position.XY();
		float dxb = dx.magnitude;
		if(dxb < 0.1) {
			needNewGoal = true;
			return;
		}
		// check if move possible
		if(!CanMove(dx)) {
			needNewGoal = true;
			return;
		}
		// move
		Vector2 move = (Time.deltaTime*speed/dxb)*dx;
		this.transform.position += move.XY0();
//		Vector2 force = (flyForce/dxb)*dx;
//		rigidbody2D.AddForce(force);
//		// limit speed for horizontal movement
//		float vx = rigidbody2D.velocity.x;
//		if(Mathf.Abs(vx) > flySpeedMax) {
//			rigidbody2D.velocity = new Vector2(Mathf.Sign(vx)*flySpeedMax, rigidbody2D.velocity.y);
//		}
		// flip if necessary
		this.transform.localScale = new Vector3((dx.x < 0 ? -1 : +1), 1, 1);
	}
}

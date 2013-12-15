using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour {

	static int moveLayerMask = (1<<8) + (1<<9); // avoid wall and objects

	public float speed = 1.0f;
	public float flyForce = 100.0f;
	public float flySpeedMax = 3.0f;

	public Vector2 Home;

	Vector2 goal;

	CircleCollider2D cc2;

	void Start() {
		cc2 = (CircleCollider2D)this.collider2D;
		goal = Home;
	}

	void SetRandomGoal() {
		goal = Home + 2.7f*Random.insideUnitCircle;
	}

	bool CanMove(Vector2 m) {
		const float SEC_STR = 0.43f;
		Vector2 p = this.transform.position.XY() + this.transform.localScale.XY().CwMult(cc2.center);
		float r = cc2.radius * 1.30f;
		float rsec = r * Mathf.Sqrt(1.0f + SEC_STR*SEC_STR);
		Vector2 mo = new Vector2(m.y, -m.x);
		RaycastHit2D hit1 = Physics2D.Raycast(p, m.normalized, r, moveLayerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(p, (m + SEC_STR*mo).normalized, rsec, moveLayerMask);
		RaycastHit2D hit3 = Physics2D.Raycast(p, (m - SEC_STR*mo).normalized, rsec, moveLayerMask);
		Debug.DrawLine(p.XY0(), (p + r*m.normalized).XY0());
		Debug.DrawLine(p.XY0(), (p + rsec*(m + SEC_STR*mo).normalized).XY0());
		Debug.DrawLine(p.XY0(), (p + rsec*(m - SEC_STR*mo).normalized).XY0());
		return !hit1 && !hit2 && !hit3;
	}
	
	void Update() {
//		Vector2 dx = goal - this.transform.position.XY();
//		float dxb = dx.magnitude;
//		if(dxb < 0.1 /*|| (this.transform.position.y < minFlightHeight && dx.y < 0)*/) {
//			SetRandomGoal();
//		}
//		else {
//			// compute move
//			Vector2 move = (Time.deltaTime*speed/dxb)*dx;
//			// check if move possible
//			if(CanMove(move)) {
//				// move
//				this.transform.position += move.XY0();
//				// flip if necessary
//				this.transform.localScale = new Vector3((dx.x < 0 ? -1 : +1), 1, 1);
//			}
//			else {
//				// new goal
//				SetRandomGoal();
//			}
//		}
		if(needNewGoal) {
			SetRandomGoal();
			needNewGoal = false;
		}
	}

	bool needNewGoal = false;

	void FixedUpdate() {
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

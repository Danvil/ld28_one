using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	static int jumpSupportLayerMask = (1<<8);
	static float jumpSupportPoint = 0.55f;

	public float walkForce = 250.0f;
	public float walkSpeedMax = 5.0f;
	bool walkIsFlipped = false;
	public float walkAirPercent = 0.4f;

	public float jumpForce = 5000;	
	public float jumpRate = 0.5f;
	private float nextJump = 0.0f;
	bool hasSupport = false;

	void Awake() {
		Globals.player = this;
	}

	void Start() {
		
	}
	
	void Update() {
	}

	void FixedUpdate() {
		Vector3 force = Vector3.zero;
		// check if agent is supported from below
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, jumpSupportPoint, jumpSupportLayerMask);
		hasSupport = hit;
		// move left/right
		float dx = Input.GetAxis("Horizontal") * walkForce;
		if(!hasSupport) {
			dx *= walkAirPercent;
		}
//		float dy = Input.GetAxis("Vertical") * SPEED;
		force += new Vector3(dx, 0, 0);
		if(Mathf.Abs(dx) > 0) {
			walkIsFlipped = (dx < 0);
		}
		// flip if necessary
		this.transform.localScale = new Vector3((walkIsFlipped ? -1 : +1), 1, 1);
		// jump
		if(hasSupport && Input.GetButton("Jump") && Time.time > nextJump) {
			force += new Vector3(0, jumpForce, 0);
			nextJump = Time.time + jumpRate;
		}
		// apply
		rigidbody2D.AddForce(force);
		// limit speed for horizontal movement
		float vx = rigidbody2D.velocity.x;
		if(Mathf.Abs(vx) > walkSpeedMax) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x)*walkSpeedMax, rigidbody2D.velocity.y);
		}

	}
}

using UnityEngine;
using System.Collections;

public class AgentMove : MonoBehaviour {
	
	static int jumpSupportLayerMask = (1<<8) + (1<<9) + (1<<10);
	static float jumpSupportRad = 0.05f;
	
	public float walkForce = 450.0f;
	public float walkBreakForce = 250.0f;
	public float walkSpeedMax = 6.0f;
	bool walkIsFlipped = false;
	public float walkAirPercent = 0.1f;
	
	public float jumpForce = 3500;	
	public float jumpRate = 0.5f;
	float nextJump = 0.0f;
	bool hasSupport = false;
	
	private Animator animator;
	AgentHealth ah;

	public bool CanJump {
		get {
			return Time.time > nextJump;
		}
	}

	private float moveDx;
	public float MoveDx {
		get { return moveDx; }
		set {
			moveDx = value;
			if(moveDx > 1) moveDx = 1;
			if(moveDx < -1) moveDx = -1;
			if(Mathf.Abs(moveDx) < 0.01) moveDx = 0;
		}
	}

	public bool DoJump { get; set; }

	bool TestForSupport() {
		return Tools.ThreeRayTest2D((CircleCollider2D)this.collider2D, this.transform, -Vector2.up, jumpSupportRad, 0.73f, jumpSupportLayerMask);
	}
	
	void Start() {
		animator = GetComponent<Animator>();
		ah = GetComponent<AgentHealth>();
	}

	void Update() {
	}
	
	void FixedUpdate() {
		if(ah.IsDead) {
			animator.SetBool("dead",  true);
			return;
		}
		else {
			animator.SetBool("dead",  false);
		}
		float vx = rigidbody2D.velocity.x;
		float vy = rigidbody2D.velocity.y;
		Vector3 force = Vector3.zero;
		// check if agent is supported from below
		hasSupport = TestForSupport();
		// move left/right
		float dx = MoveDx * walkForce;
		if(Mathf.Abs(dx) > 0) {
			if(!hasSupport) {
				dx *= walkAirPercent;
			}
			force += new Vector3(dx, 0, 0);
			walkIsFlipped = (dx < 0);
		}
		else {
			float ax = -Mathf.Sign(vx)*walkBreakForce;
			if(!hasSupport) {
				ax *= walkAirPercent;
			}
			force += new Vector3(ax, 0, 0);
		}
		// flip if necessary
		this.transform.localScale = new Vector3((walkIsFlipped ? -1 : +1), 1, 1);
		// jump
		bool isJumping = hasSupport && DoJump && Time.time > nextJump;
		if(isJumping) {
			force += new Vector3(0, jumpForce, 0);
			nextJump = Time.time + jumpRate;
		}
		// apply
		rigidbody2D.AddForce(force);
		// limit speed for horizontal movement
		if(Mathf.Abs(vx) > walkSpeedMax) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x)*walkSpeedMax, rigidbody2D.velocity.y);
		}
		animator.SetBool("walk",  hasSupport && Mathf.Abs(vx) > 0.3); // moving left/right
		animator.SetBool("fall", !hasSupport && vy < 0); // moving down
		animator.SetBool("jump", !hasSupport && vy > 0); // moving up
	}
}

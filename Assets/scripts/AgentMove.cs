using UnityEngine;
using System.Collections;

public class AgentMove : MonoBehaviour {
	
	int JUMP_SUPPORT_LAYER_MASK;
	static float jumpSupportRad = 0.05f;
	
	public float walkForce = 450.0f;
	public float walkBreakForce = 250.0f;
	public float walkSpeedMax = 6.0f;
	bool walkIsFlipped = false;
	public float walkAirPercent = 0.1f;

	public bool UseRun = false;
	
	public float jumpForce = 3500;	
	public float jumpRate = 0.5f;
	float nextJump = 0.0f;
	bool hasSupport = false;

	Agent agent;

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
		return Tools.ThreeRayTest2D((CircleCollider2D)this.collider2D, this.transform, -Vector2.up, jumpSupportRad, 0.73f, JUMP_SUPPORT_LAYER_MASK);
	}

	void Awake() {
		agent = GetComponent<Agent>();
	}
	
	void Start() {
		JUMP_SUPPORT_LAYER_MASK =
			(1<<LayerMask.NameToLayer("Wall")) +
			(1<<LayerMask.NameToLayer("Objects")) +
			(1<<LayerMask.NameToLayer("Scenery")) +
			(1<<LayerMask.NameToLayer("Agents"));
	}

	void Update() {
	}
	
	void FixedUpdate() {
		if(agent.health.IsDead) {
			agent.animator.SetBool("dead",  true);
			return;
		}
		else {
			agent.animator.SetBool("dead",  false);
		}
		bool running = UseRun && agent.HasSpeed;
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
		if(running) {
			dx *= 2.0f;
		}
		// flip if necessary
		this.transform.localScale = new Vector3((walkIsFlipped ? -1 : +1), 1, 1);
		// jump
		bool isJumping = hasSupport && DoJump && CanJump;
		if(isJumping) {
			float jf = jumpForce;
			if(agent.HasJump) {
				jf *= 1.41f;
			}
			force += new Vector3(0, jf, 0);
			nextJump = Time.time + jumpRate;
		}
		// apply
		rigidbody2D.AddForce(force);
		// limit speed for horizontal movement
		float speedMax = walkSpeedMax * (running ? 2.0f : 1.0f);
		if(Mathf.Abs(vx) > speedMax) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x)*speedMax, rigidbody2D.velocity.y);
		}
		agent.animator.SetBool("walk",  hasSupport && Mathf.Abs(vx) > 0.3); // moving left/right
		agent.animator.SetBool("fall", !hasSupport && vy < 0); // moving down
		agent.animator.SetBool("jump", !hasSupport && vy > 0); // moving up
	}
}

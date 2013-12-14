using UnityEngine;
using System.Collections;

public class AgentMove : MonoBehaviour {
	
	static int jumpSupportLayerMask = (1<<8) + (1<<9) + (1<<10);
	static float jumpSupportPoint = 0.55f;
	
	public float walkForce = 250.0f;
	public float walkSpeedMax = 5.0f;
	bool walkIsFlipped = false;
	public float walkAirPercent = 0.4f;
	
	public float jumpForce = 5000;	
	public float jumpRate = 0.5f;
	private float nextJump = 0.0f;
	bool hasSupport = false;
	
	private Animator animator;
	
	void Start() {
		animator = GetComponent<Animator>();
	}
	
	void Update() {
	}

	public bool CanJump {
		get {
			return Time.time > nextJump;
		}
	}

	public float MoveDx { get; set; }
	public bool DoJump { get; set; }
	
	void FixedUpdate() {
		Vector3 force = Vector3.zero;
		// check if agent is supported from below
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, jumpSupportPoint, jumpSupportLayerMask);
		hasSupport = hit;
		// move left/right
		float dx = MoveDx * walkForce;
		if(Mathf.Abs(dx) > 0) {
			if(!hasSupport) {
				dx *= walkAirPercent;
			}
			force += new Vector3(dx, 0, 0);
			walkIsFlipped = (dx < 0);
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
		float vx = rigidbody2D.velocity.x;
		float vy = rigidbody2D.velocity.y;
		if(Mathf.Abs(vx) > walkSpeedMax) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x)*walkSpeedMax, rigidbody2D.velocity.y);
		}
		animator.SetBool("walk",  hasSupport && Mathf.Abs(vx) > 0); // moving left/right
		animator.SetBool("fall", !hasSupport && vy < 0); // moving down
		animator.SetBool("jump", !hasSupport && vy > 0); // moving up
	}
}

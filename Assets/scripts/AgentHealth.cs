using UnityEngine;
using System.Collections;

public class AgentHealth : MonoBehaviour {

	public float healthMax = 10.0f;
	public GameObject pfExplosion;

	private float health;
	public float Health {
		get {
			return health;
		}
		set {
			if(IsDead) {
				return;
			}
			health = value;
			if(health <= 0) {
				Die();
			}
		}
	}
	
	public bool IsDead { get; private set; }

	public bool IsPlayer {
		get {
			return (GetComponent<Player>() != null);
		}
	}

	public void Die() {
		if(IsDead) {
			return;
		}
		if(!IsDead) {
			health = 0;
			IsDead = true;
		}
		// play animation
		StartCoroutine(FireDeathExplosion());
		// make unmovable
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.gravityScale = 1.0f;
		if(!IsPlayer) {
			StartCoroutine(DestroyMe());
		}
	}

	IEnumerator FireDeathExplosion() {
		GameObject a = (GameObject)Instantiate(pfExplosion);
		float dur = 0.0f;
		foreach(var ps in a.GetComponentsInChildren<ParticleSystem>()) {
			ps.renderer.sortingLayerName = "Fx";
			dur = Mathf.Max(dur, ps.duration);
		}
		a.transform.position = this.transform.position + new Vector3(0,0,-3);
		yield return new WaitForSeconds(dur);
		Destroy(a);
	}

	IEnumerator DestroyMe() {
		yield return new WaitForSeconds(10.0f);
		Destroy(this.gameObject);
	}

	void Start() {
		IsDead = false;
		Health = healthMax;
	}
	
	void Update() {
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(IsDead) {
			return;
		}
		float v = collision.relativeVelocity.magnitude;
		if(!IsPlayer && v > 8.0f) {
			Die();
		}
	}

}

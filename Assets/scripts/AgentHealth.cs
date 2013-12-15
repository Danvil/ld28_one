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
		StartCoroutine(CreateDeathExplosion());
		// make unmovable
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.gravityScale = 1.0f;
//		if(!IsPlayer) {
//			StartCoroutine(DestroyMe());
//		}
	}

	IEnumerator CreateDeathExplosion() {
		GameObject a = (GameObject)Instantiate(pfExplosion);
		a.transform.position = this.transform.position + new Vector3(0,0,-3);
		float dur = 0.0f;
		foreach(var ps in a.GetComponentsInChildren<ParticleSystem>()) {
			ps.renderer.sortingLayerName = "Fx";
			dur = Mathf.Max(dur, ps.duration);
		}
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

	float EnergyToDamage(float e) {
		// 3*3*10 = 90
		// 6*6*10 = 360
		return Mathf.Max(0,e-100.0f) / 100.0f;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(IsDead) {
			return;
		}
		float v = collision.relativeVelocity.magnitude;
		if(!IsPlayer) {
			// kinetic energy
			float e = 0.5f*collision.rigidbody.mass*v*v;
			// energy to damage
			float dmg = EnergyToDamage(e);
			// take damage
			Health -= dmg;
		}
	}

}

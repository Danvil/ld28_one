using UnityEngine;
using System.Collections;

public class AgentHealth : MonoBehaviour {

	public float healthMax = 10.0f;
	public GameObject pfExplosion;

	public float energyDmgBase = 450.0f;
	public float energyDmgRate = 200.0f;

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
		// show particles
		StartCoroutine(Tools.CreateParticleEffect(pfExplosion, this.transform.position));
		// make unmovable
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.gravityScale = 1.0f;
		this.collider2D.sharedMaterial = agent.pfObjectMaterial;
		this.renderer.sortingLayerName = "Objects";
		//		if(!IsPlayer) {
//			StartCoroutine(DestroyMe());
//		}
	}

	IEnumerator DestroyMe() {
		yield return new WaitForSeconds(10.0f);
		Destroy(this.gameObject);
	}

	Agent agent;

	void Start() {
		IsDead = false;
		Health = healthMax;
		agent = GetComponent<Agent>();
	}
	
	void Update() {
	}

	float EnergyToDamage(float e) {
		// 3*3*10 = 90
		// 4*4*10 = 160
		// 6*6*10 = 360
		// 8*8*10 = 640
		return Mathf.Max(0, e-energyDmgBase) / energyDmgRate;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(IsDead) {
			return;
		}
		if(collision.rigidbody == null) {
			return;
		}
		float v = collision.relativeVelocity.magnitude;
		float dmgScl = (IsPlayer ? 0.5f : 1.0f);
		// kinetic energy
		float e = 0.5f*collision.rigidbody.mass*v*v;
		// energy to damage
		float dmg = dmgScl * EnergyToDamage(e);
		if(dmg == 0) {
			return;
		}

		if(this.gameObject == Globals.player.gameObject) {
			Globals.messages.Show(string.Format("e={0:0.00}, d={1:0.00}", e, dmg), 0.3f);
		}
		
		// take damage
		Health -= dmg;
	}

}

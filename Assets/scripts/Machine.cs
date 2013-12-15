using UnityEngine;
using System.Collections;

public class Machine : MonoBehaviour {

	public GameObject pfPill;

	public int num = 1;
	
	GameObject goPill;

	bool hasPill = true;

	void Awake() {
		goPill = transform.FindChild("pill").gameObject;
	}

	void Start() {
		foreach(Number q in GetComponentsInChildren<Number>()) {
			q.Num = num;
		}
	}

	void Update() {
		if(!hasPill) {
			return;
		}
		float dist = (Globals.player.transform.position - goPill.transform.position).XY().magnitude;
		if(dist < 0.3f) {
			// eat pill
			StartCoroutine(Tools.CreateParticleEffect(pfPill, this.transform.position));
			// give skill
			switch(num) {
			case 1: Globals.player.agent.HasDump = true; break;
			case 2: Globals.player.agent.HasKnive = true; break;
			case 3: Globals.player.agent.HasJump = true; break;
			case 4: Globals.player.agent.HasSpeed = true; break;
			case 5: Globals.player.agent.HasCarry = true; break;
			case 6: Globals.player.agent.HasRainbow = true; break;
			default: break;
			}
			// remove pill
			hasPill = false;
			goPill.renderer.enabled = false;
		}
	}

}

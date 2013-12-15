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

	IEnumerator ShowSkillMessage() {
		const float MSG_DUR = 1.76f;
		string[] msgs = {
			"#1: For loosers only",
			"#2: Secret ninja skills!",
			"#3: Jump high into the sky!",
			"#4: Fast as lightning",
			"#5: Strength of a mountain",
			"#6: Glamor!"
		};
		Globals.messages.Show(msgs[num-1], MSG_DUR);
		yield return new WaitForSeconds(Globals.messages.GetCurrentDuration()-0.05f);
		string[] answer = {
			"\"I can't stand it anymore!\"",
			"\"They will tast my steel.\"",
			"\"No one can keep my down!\"",
			"\"The hand is faster than the eye.\"",
			"\"Nothing can stop me now!\"",
			"\"Let them eat cake\""
		};
		Globals.messages.Show(answer[num-1], MSG_DUR);
	}

	void Update() {
		if(!hasPill) {
			return;
		}
		float dist = (Globals.player.transform.position - goPill.transform.position).XY().magnitude;
		if(dist < 0.5f) {
			// eat pill
			StartCoroutine(Tools.CreateParticleEffect(pfPill, this.transform.position));
			// give skill
			switch(num) {
			case 1:
				Globals.player.agent.HasDump = true;
				break;
			case 2:
				Globals.player.agent.HasKnive = true;
				break;
			case 3:
				Globals.player.agent.HasJump = true;
				break;
			case 4:
				Globals.player.agent.HasSpeed = true;
				break;
			case 5:
				Globals.player.agent.HasCarry = true;
				break;
			case 6:
				Globals.player.agent.HasRainbow = true;
				break;
			default: break;
			}
			// message
			StartCoroutine(ShowSkillMessage());
			// remove pill
			hasPill = false;
			goPill.renderer.enabled = false;
		}
	}

}

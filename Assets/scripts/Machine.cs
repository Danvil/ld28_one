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
			"#2: Strength of a mountain\nUse 'k' to pick and throw.",
			"#3: Jump high into the sky!\nJump again in mid air.",
			"#4: Secret ninja skills!\nPress 'j' to slice.",
			"#5: Fast as lightning\nHold 'shift' to run.",
			"#6: Glamor!",
			"#7: The ultimate Power!",
		};
		Globals.messages.Show(msgs[num-1], MSG_DUR);
		yield return new WaitForSeconds(Globals.messages.GetCurrentDuration()-0.05f);
		string[] answer = {
			"\"I can't stand it anymore!\"",
			"\"Nothing can stop me now!\"",
			"\"No one can keep my down!\"",
			"\"They will taste my steel.\"",
			"\"The hand is faster than the eye.\"",
			"\"Let them eat cake\"",
			"\"The force is strong with this one\""
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
				Globals.player.agent.HasCarry = true;
				break;
			case 3:
				Globals.player.agent.HasJump = true;
				break;
			case 4:
				Globals.player.agent.HasKnive = true;
				break;
			case 5:
				Globals.player.agent.HasSpeed = true;
				break;
			case 6:
				Globals.player.agent.HasRainbow = true;
				break;
			case 7:
				Globals.player.agent.HasUltimate = true;
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

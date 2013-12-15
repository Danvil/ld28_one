using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public int level = 0;

	ActionDelay adTeleport = new ActionDelay(3.0f);
	
	void Start() {
		foreach(Number q in GetComponentsInChildren<Number>()) {
			q.Num = level;
		}
	}
	
	void Update() {
		adTeleport.Update();
		if(Input.GetButton("Fire2") && adTeleport.Available) {
			// check if near portal
			float dx = Mathf.Abs(Globals.player.transform.position.x - this.transform.position.x);
			float dy = Mathf.Abs(Globals.player.transform.position.y - this.transform.position.y);
			if(dx < 2.0 && dy < 2.5) {
				adTeleport.Execute();
				Globals.game.LoadLevel(level);
			}
		}
	}

}

using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public int level = 0;

	public Texture2D defLevelTex;

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
				// go through portal
				// delete current level
				GameObject now = GameObject.Find("Level");
				Destroy(now);
				// create new
				Level lvl = CreateLevel(level);
				Globals.lvlGen.CreateGameobjects(lvl);
				// TODO somehow transfer player info
				// correct player position
				// find correct portal
				Portal[] portals = GameObject.FindObjectsOfType<Portal>();
				foreach(Portal p in portals) {
					if(p.level == Globals.levelId) {
						Globals.player.transform.position = p.transform.position;
						break;
					}
				}
				// go through
				Globals.levelId = level;
			}
		}
	}

	Level CreateLevel(int i) {
		if(i == 0) {
			return Globals.lvlGen.CreateLevelFromTex(defLevelTex);
		}
		else {
			return Globals.lvlGen.CreateRandomLevel();
		}

	}
}

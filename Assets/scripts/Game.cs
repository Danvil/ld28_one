using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public Texture2D defLevelTex;
	
	void Awake() {
		Globals.game = this;
	}

	void Start() {
		// generate level
		LoadLevel(0);
		// set player
		Globals.player.agent.HasKnive = false;
		Globals.player.agent.HasJump = false;
		Globals.player.agent.HasSpeed = false;
		Globals.player.agent.HasCarry = false;
		Globals.player.agent.HasRainbow = false;
	}
	
	void Update() {
	
	}

	public void LoadLevel(int i) {
		// go through portal
		// delete current level
		GameObject now = GameObject.Find("Level");
		Destroy(now);
		// create new
		Level lvl = CreateLevel(i);
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
		Globals.levelId = i;
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

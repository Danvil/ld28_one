using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public Texture2D defLevelTex;
	public UnityEngine.GameObject plPlayer;

	void Awake() {
		Globals.game = this;
	}

	void Start() {
		// generate player
		GameObject pgo = (GameObject)Instantiate(plPlayer);
		Globals.player = pgo.GetComponentInChildren<Player>();
		// set player
		Globals.player.agent.HasKnive = false;
		Globals.player.agent.HasJump = false;
		Globals.player.agent.HasSpeed = false;
		Globals.player.agent.HasCarry = false;
		Globals.player.agent.HasRainbow = false;
		// generate level
		LoadLevel(0);
	}
	
	void Update() {
	
	}

	public void LoadLevel(int i) {
		// go through portal
		// delete current level
		GameObject old_level_go = GameObject.Find("Level");
		Destroy(old_level_go);
		// create new
		Level lvl = CreateLevel(i);
		Globals.lvlGen.CreateGameobjects(lvl);
		// set player position by finding correct portal
		foreach(Portal p in GameObject.FindObjectsOfType<Portal>()) {
			if(p.level == Globals.levelId) {
				Globals.player.transform.position = p.transform.position;
				break;
			}
		}
		if(i != 0) {
			// set machine id
			foreach(Machine m in GameObject.FindObjectsOfType<Machine>()) {
				m.num = i;
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

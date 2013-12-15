using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public Texture2D defLevelTex;
	public UnityEngine.GameObject plPlayer;

	void Awake() {
		Globals.game = this;
	}

	void Start() {
		StartGame();
	}

	bool showedGameOverMessage;
	
	void Update() {
		if(Globals.player.agent.health.IsDead) {
			if(!showedGameOverMessage) {
				StartCoroutine(ShowGameOver());
				showedGameOverMessage = true;
			}
			if(Input.GetButton("Fire2")) {
				// restert
				RestartGame();
			}
		}
	}

	void RestartGame() {
		GameObject.Destroy(Globals.player);
		DestroyLevel();
		StartGame();
	}

	void StartGame() {
		showedGameOverMessage = false;
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

	IEnumerator ShowGameOver() {
		Globals.messages.Show("Game Over", 1);
		yield return new WaitForSeconds(Globals.messages.GetCurrentDuration()-0.05f);
		Globals.messages.Show("Press 'l' to restart", 100000);
	}

	void DestroyLevel() {
		GameObject old_level_go = GameObject.Find("Level");
		Destroy(old_level_go);
	}

	public void LoadLevel(int i) {
		// go through portal
		// delete current level
		DestroyLevel();
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
		else if(i == 1) {
			// get dumb
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 3, nh = 1,
				droneProp = 0.0f, agentProp = 0.05f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else if(i == 2) {
			// find knive
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 4, nh = 2,
				droneProp = 0.0f, agentProp = 0.05f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else if(i == 4) {
			// get speed
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 4, nh = 3,
				droneProp = 0.0f, agentProp = 0.15f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else if(i == 3) {
			// get jump
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 8, nh = 2,
				droneProp = 0.01f, agentProp = 0.15f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else if(i == 5) {
			// get throw
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 4, nh = 3,
				droneProp = 0.01f, agentProp = 0.15f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else if(i == 6) {
			// get glamor
			LevelGenerator.Parameters par = new LevelGenerator.Parameters() {
				nw = 7, nh = 5,
				droneProp = 0.01f, agentProp = 0.15f
			};
			return Globals.lvlGen.CreateRandomLevel(par);
		}
		else {
			return Globals.lvlGen.CreateRandomLevel(
				LevelGenerator.Parameters.Defaults());
		}
		
	}
}

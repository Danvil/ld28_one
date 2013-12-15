using UnityEngine;
using System.Collections.Generic;

public class Healthbar : MonoBehaviour {

	public Sprite heartFull, heartHalf, heartEmpty;

	public GameObject pfHeart;

	List<GameObject> hearts = new List<GameObject>();

	void Start() {
	
	}

	void SetHeart(GameObject go, int t) {
		Sprite s = (t == 0 ? heartEmpty : (t == 1 ? heartHalf : heartFull));
		((SpriteRenderer)go.renderer).sprite = s;
	}
	
	void Update() {
		// create hearts
		float hmax = Globals.player.agent.health.healthMax;
		int hnum = Mathf.FloorToInt(hmax / 3.0f);
		if(hnum != hearts.Count) {
			foreach(var q in hearts) {
				Destroy(q);
			}
			for(int i=0; i<hnum; i++) {
				GameObject go = (GameObject)Instantiate(pfHeart);
				hearts.Add(go);
			}
		}
		float hnow = Globals.player.agent.health.Health;
		float p = hnow / hmax;
		int n = Mathf.RoundToInt(p * 2.0f * hnum);
		// update hearts
		const float SIZE = 2.0f * 17.0f;
		const float BORDER = 10.0f + 0.5f * SIZE;
		const float PAD = 3.0f;
		for(int i=0; i<hnum; i++) {
			GameObject go = hearts[i];
			Vector3 screenpos = new Vector3(BORDER + i*(SIZE+PAD), Screen.height - BORDER, Globals.camera.camera.nearClipPlane);
			go.transform.position = Globals.camera.camera.ScreenToWorldPoint(screenpos);
			go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);
			int t = (2*i+1 < n ? 2 : (2*i < n ? 1 : 0));
			if(i == 0 && t == 0) {
				// show 0 hearts only if dead
				if(!Globals.player.agent.health.IsDead) {
					t = 1;
				}
			}
			SetHeart(go, t);
		}
	}
}

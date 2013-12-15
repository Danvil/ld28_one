using UnityEngine;
using System.Collections;

public class Messages : MonoBehaviour {

	public GUIStyle guistyle;

	string currText = "";
	float currDur = 0;

	public void Show(string text, float dur) {
		currText = text;
		currDur = dur;
	}

	void Awake() {
		Globals.messages = this;
	}

	void Start() {
	}
	
	void Update() {
		currDur -= Time.deltaTime;
	}

	void OnGUI() {
		if(currDur > 0) {
			float w = 400.0f;
			float h = 140.0f;
			Rect windowRect = new Rect(0.5f*(Screen.width - w), 0.10f*(Screen.height - h), w, h);	
			GUI.Label(windowRect, currText, guistyle);
		}
	}
}

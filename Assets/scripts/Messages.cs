using UnityEngine;
using System.Collections;

public class Messages : MonoBehaviour {

	public GUIStyle guistyle;

	string currText = "#3: Jump high into the sky!";
	float currDur = 0;

	string textType = "";
	int textTypeNext = 0;

	public float letterTime = 0.07f;

	float letterTick = 0;

	public float GetCurrentDuration() {
		return Mathf.Max(0.0f, currDur);
	}

	public void Show(string text, float dur) {
		currText = text;
		currDur = dur + text.Length * letterTime;
		textType = "";
		textTypeNext = 0;
	}

	void Awake() {
		Globals.messages = this;
	}

	void Start() {
	}
	
	void Update() {
		currDur -= Time.deltaTime;
		if(currDur > 0) {
			letterTick -= Time.deltaTime;
			if(letterTick < 0) {
				letterTick = letterTime;
				if(textTypeNext < currText.Length) {
					textType += currText[textTypeNext];
					textTypeNext ++;
				}
			}
		}
	}

	void OnGUI() {
		if(currDur > 0) {
			float w = 400.0f;
			float h = 140.0f;
			Rect windowRect = new Rect(0.5f*(Screen.width - w), 0.10f*(Screen.height - h), w, h);	
			GUI.Label(windowRect, textType, guistyle);
		}
	}
}

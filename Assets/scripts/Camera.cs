using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	void Awake() {
		Globals.camera = this;
	}
	
	void Start() {
	
	}
	
	void Update() {
		float x = Globals.player.transform.position.x;
		float y = Globals.player.transform.position.y;
		float z = this.transform.position.z;
		this.transform.position = new Vector3(x,y,z);
	}
}

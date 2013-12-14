using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	static float SPEED = 1;

	void Start() {
	
	}
	
	void Update() {
		float dy = Input.GetAxis("Vertical") * SPEED;
		float dx = Input.GetAxis("Horizontal") * SPEED;
		camera.transform.position += new Vector3(dx, dy, 0);
	}
}

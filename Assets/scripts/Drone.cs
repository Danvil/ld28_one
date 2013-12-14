using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour {

	public float speed = 1.0f;
	public float minFlightHeight = 7.5f;

	public Vector2 Home;

	Vector2 goal;

	void Start() {
		goal = Home;
	}

	void SetRandomGoal() {
		goal = Home + 2.7f*Random.insideUnitCircle;
	}
	
	void Update() {
		Vector2 dx = goal - this.transform.position.XY();
		float dxb = dx.magnitude;
		if(dxb < 0.1 || (this.transform.position.y < minFlightHeight && dx.y < 0)) {
			SetRandomGoal();
		}
		else {
			this.transform.position += (Time.deltaTime*speed/dxb)*dx.XY0();
			// flip if necessary
			this.transform.localScale = new Vector3((dx.x < 0 ? -1 : +1), 1, 1);
		}
	}
}

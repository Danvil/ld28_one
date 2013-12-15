using UnityEngine;
using System.Collections;

public class Machine : MonoBehaviour {

	public int num = 1;

	void Start() {
		foreach(Number q in GetComponentsInChildren<Number>()) {
			q.Num = num;
		}
	}

	void Update() {
	
	}
}

using UnityEngine;
using System.Collections;

public class Number : MonoBehaviour {

	public Sprite[] sprites;

	public int number = 0;
	public int Num {
		get {
			return number;
		}
		set {
			number = value;
			((SpriteRenderer)this.renderer).sprite = sprites[number-1];
		}
	}

	void Start() {
		Num = number;
	}
	
	void Update() {
	}

}

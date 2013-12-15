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
			if(1 <= number && number <= 9) {
				this.renderer.enabled = true;
				((SpriteRenderer)this.renderer).sprite = sprites[number-1];
			}
			else {
				this.renderer.enabled = false;
			}
		}
	}

	void Start() {
		Num = number;
	}
	
	void Update() {
	}

}

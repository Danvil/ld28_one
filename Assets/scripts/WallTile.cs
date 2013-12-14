using UnityEngine;
using System.Collections;

public class WallTile : MonoBehaviour {

	public Sprite[] sprites;

	void Start() {
	
	}
	
	void Update() {
	
	}

	public void SetTile(int neighbours) {
		// neighbours: 1 = top, 2 = right, 4 = bottom, 8 = left
		// 0 = free, 1 = set
//		int[] mapping = {
//			 0, // O
//			 1, // U
//			 2, // C
//			 3, // L
//			 4, // M
//			 5, // ||
//			 6, // |^
//			 7, // |X
//			 8, // !C
//			 9, // _|
//			10, // r||
//			11, // _
//			12, // ^|
//			13, // X|
//			14, // ^
//			15, // X
//		}; // NICE :)
		((SpriteRenderer)this.renderer).sprite = sprites[neighbours];
	}
}

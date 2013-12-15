using UnityEngine;

static public class Tools
{
	static public Vector2 XY(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}

	static public Vector3 XY0(this Vector2 v) {
		return new Vector3(v.x, v.y, 0);
	}

	static public Vector2 CwMult(this Vector2 a, Vector2 b) {
		return new Vector2(a.x * b.x, a.y * b.y);
	}

}

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

	static public bool ThreeRayTest2D(CircleCollider2D cc2, Transform t, Vector2 m, float rAdd, float str, int mask) {
		Vector2 p = t.position.XY() + t.localScale.XY().CwMult(cc2.center);
		float r = cc2.radius + rAdd;
		float rsec = r * Mathf.Sqrt(1.0f + str*str);
		Vector2 mo = new Vector2(m.y, -m.x);
		Vector2 d1 = m.normalized;
		Vector2 d2 = ((1.0f-str)*m + str*mo).normalized;
		Vector2 d3 = ((1.0f-str)*m - str*mo).normalized;
		RaycastHit2D hit1 = Physics2D.Raycast(p, d1, r, mask);
		RaycastHit2D hit2 = Physics2D.Raycast(p, d2, rsec, mask);
		RaycastHit2D hit3 = Physics2D.Raycast(p, d3, rsec, mask);
		Debug.DrawLine(p.XY0(), (p+d1).XY0());
		Debug.DrawLine(p.XY0(), (p+d2).XY0());
		Debug.DrawLine(p.XY0(), (p+d3).XY0());
		return hit1 || hit2 || hit3;
	}

}

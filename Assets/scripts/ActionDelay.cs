
public class ActionDelay
{
	public float rate;

	public float timer;

	public ActionDelay(float rate) {
		this.rate = rate;
		timer = rate;
	}

	public bool Available {
		get {
			return timer < 0;
		}
	}

	public void Execute() {
		timer = rate;
	}

	public void Update() {
		timer -= UnityEngine.Time.deltaTime;
	}
}

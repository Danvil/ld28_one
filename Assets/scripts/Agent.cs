using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	public Animator animator;
	public AgentMove move;
	public AgentHealth health;
	public AgentCarry carry;
	public AgentSlice slice;

	public bool IsDead { get { return health.IsDead; } }

	void Start() {
		animator = GetComponent<Animator>();
		move = GetComponent<AgentMove>();
		health = GetComponent<AgentHealth>();
		carry = GetComponent<AgentCarry>();
		slice = GetComponent<AgentSlice>();
	}
	
	void Update() {
	
	}
}

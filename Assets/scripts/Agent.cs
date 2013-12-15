using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	public Animator animator;
	public AgentMove move;
	public AgentHealth health;
	public AgentCarry carry;
	public AgentSlice slice;

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

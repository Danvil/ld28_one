﻿using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	public Animator animator;
	public AgentMove move;
	public AgentHealth health;
	public AgentCarry carry;
	public AgentSlice slice;
	public PhysicsMaterial2D pfObjectMaterial;

	public bool IsDead { get { return health.IsDead; } }

	public bool HasDump {
		get; set;
	}

	public bool HasNumberOne {
		get { return false; }
		set { }
	}
	
	public bool HasKnive {
		get { return slice.enabled; }
		set { slice.enabled = value; }
	}

	public bool HasJump {
		get; set;
	}

	public bool HasSpeed {
		get; set;
	}

	public bool HasCarry {
		get { return carry.enabled; }
		set { carry.enabled = value; }
	}

	public bool HasRainbow {
		get { return false; }
		set { }
	}

	public bool HasUltimate {
		get; set;
	}
	
	void Awake() {
		animator = GetComponent<Animator>();
		move = GetComponent<AgentMove>();
		health = GetComponent<AgentHealth>();
		carry = GetComponent<AgentCarry>();
		slice = GetComponent<AgentSlice>();
	}
	
	void Start() {
	}
	
	void Update() {
	
	}
}

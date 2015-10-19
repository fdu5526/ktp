﻿using UnityEngine;
using System.Collections;

public class SwarmMemberAI : SwarmMember {

	public int currentWaypointIndex;
	float currentEncircleSqDistance;
	float currentEncircleDirection;
	

	const float tackleDistance = 50f;


	// Use this for initialization
	void Start () {
		currentState = State.RunToward;

		currentEncircleSqDistance = NewEncircleDistance;
		currentEncircleDirection = NewEncircleDirection;

		base.Initialize();
	}


	float NewEncircleDistance { get { return UnityEngine.Random.Range(100f, 300f); } }
	float NewEncircleDirection { get { return UnityEngine.Random.value > 0.5f ? 1f : -1f; } }

	
	Vector3 FindTarget (ref bool isKtp) {
		Vector3 p = GetComponent<Transform>().position;
		int w = Ktp.GetComponent<Ktp>().currentWaypointIndex;

		isKtp = false;
		Vector3 d1 = Ktp.GetComponent<Transform>().position - (Vector3)p;
		Vector3 d2 = waypoints[currentWaypointIndex].position - (Vector3)p;




		if (currentWaypointIndex == w - 1 ||
				d1.sqrMagnitude < d2.sqrMagnitude) {
			isKtp = true;
			return d1;
		} else if (d2.sqrMagnitude < 10f) {
			if (currentWaypointIndex != 0 && 
					currentWaypointIndex >= w) {
				currentWaypointIndex--;
			}
			return waypoints[currentWaypointIndex].position - (Vector3)p;
		} else {
			return d2;
		}
	}


	void SeekTarget (Vector3 d, bool isKtp) {

		float sm = d.sqrMagnitude;
		if (isKtp) {
			// state transitions
			if (currentState == State.RunToward && // RunToward -> Tackle
					sm < tackleDistance &&
					tackleCooldownTimer.IsOffCooldown()) {
				TackleDirection = d.normalized;
				Tackle();
			} else if (currentState == State.Encircle && // Encircle -> Tackle
								 sm < tackleDistance && 
								 tackleCooldownTimer.IsOffCooldown()) {
				TackleDirection = d.normalized;
				Tackle();
			} else if (currentState == State.RunToward && sm <= currentEncircleSqDistance) { // RunToward -> Encircle
				currentState = State.Encircle;
				currentEncircleDirection = NewEncircleDirection;
			} else if (currentState == State.Encircle && sm > currentEncircleSqDistance * 1.5f) { // Encircle -> RunToward
				currentState = State.RunToward;
			} else if (currentState == State.Tackle && tackleDurationTimer.IsOffCooldown()) { // Tackle -> RunToward
				currentState = State.RunToward;
				currentEncircleSqDistance = NewEncircleDistance;
				attackStunTimer.Reset();
			}
		}
		


		if (currentState != State.Tackle) {

			if (currentState == State.Encircle) {
				if (currentEncircleDirection > 0f) { 
					d = new Vector3(d.x * 0.17f + d.z * 0.985f, 0f, d.x * -0.985f + d.z * 0.17f);
				} else {
					d = new Vector3(d.x * 0.17f + d.z * -0.985f, 0f, d.x * 0.985f + d.z * 0.17f);
				}
	  	}	

	  	d = d.normalized * defaultSpeed;
	  	GetComponent<Rigidbody>().velocity = new Vector3(d.x, 0f, d.z);

			float y = 0f; // TODO calculate correct Y here
	  	GetComponent<Transform>().eulerAngles = new Vector3(0f, y, 0f);
		}
	}


	protected override void Respawn () {
		int w = NearestRespawnPoint();
		GameObject g = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Swarm Member"));

		Vector2 r = UnityEngine.Random.insideUnitCircle * 5f;
		g.GetComponent<Transform>().position = waypoints[w].position + new Vector3(r.x, 0f, r.y);
		g.GetComponent<SwarmMemberAI>().currentWaypointIndex = w - 1;
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (currentState == State.Dead) {
			return;
		}

		if (currentState != State.Disabled &&
				attackStunTimer.IsOffCooldown()) {
			bool b = false;
			Vector3 d = FindTarget(ref b);
			SeekTarget(d, b);
		} else if (currentState == State.Disabled && 
							 respawnTimer.IsOffCooldown()) {
			currentState = State.Dead;
      Respawn();
		}
	}
}

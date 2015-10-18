using UnityEngine;
using System.Collections;

public class SwarmMemberAI : SwarmMember {

	float currentEncircleSqDistance;
	float currentEncircleDirection;
	Transform[] waypoints;

	const float tackleDistance = 50f;


	// Use this for initialization
	void Start () {
		currentState = State.RunToward;

		waypoints = new Transform[6];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints[i] = GameObject.Find("Waypoints/Waypoint" + i).GetComponent<Transform>();
		}

		currentEncircleSqDistance = NewEncircleDistance;
		currentEncircleDirection = NewEncircleDirection;

		base.Initialize();
	}


	float NewEncircleDistance { get { return UnityEngine.Random.Range(100f, 300f); } }
	float NewEncircleDirection { get { return UnityEngine.Random.value > 0.5f ? 1f : -1f; } }

	

	void SeekTarget () {

		Vector3 p = GetComponent<Transform>().position;
		Vector3 d = Ktp.GetComponent<Transform>().position - (Vector3)p;
		float sm = d.sqrMagnitude;

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
			hitStunTimer.Reset();
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


	// Update is called once per frame
	void FixedUpdate () {
		if (currentState != State.Disabled) {
			SeekTarget();
		}
	}
}

using UnityEngine;
using System.Collections;

public class Ktp : MonoBehaviour {

	const float defaultSpeed = 5f;
	const float attackSpeed = 30f;

	Transform[] waypoints;
	int currentWaypointIndex;

	Timer attackCooldownTimer;
	Timer attackDurationTimer;

	GameObject ktpAttack;


	// Use this for initialization
	void Start () {
		waypoints = new Transform[6];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints[i] = GameObject.Find("Waypoints/Waypoint" + i).GetComponent<Transform>();
		}

		ktpAttack = GameObject.Find("Ktp/KtpAttack");
		ktpAttack.SetActive(false);
		attackCooldownTimer = new Timer(0.5f);
		attackDurationTimer = new Timer(0.1f);

		currentWaypointIndex = 0;
	}


	void Attack (float time) {

		// TODO
		Vector3 tp = waypoints[currentWaypointIndex].GetComponent<Transform>().position;
		Vector3 pp = GetComponent<Transform>().position;
		pp = new Vector3(pp.x, 0f, pp.z);
		Vector3 v = tp - pp;
		v = new Vector3(v.x, 1f, v.z);
		v = v.normalized * attackSpeed;

		ktpAttack.GetComponent<Transform>().position = GetComponent<Transform>().position;

		

		ktpAttack.GetComponent<Rigidbody>().velocity = v;

		ktpAttack.SetActive(true);
		attackDurationTimer.Reset(time);
	}




	void WalkToWaypoint () {

		if (currentWaypointIndex == waypoints.Length) {
			return;
		}

		Vector3 tp = waypoints[currentWaypointIndex].GetComponent<Transform>().position;
		Vector3 pp = GetComponent<Transform>().position;
		Vector3 v = tp - pp;

		GetComponent<Rigidbody>().velocity = v.normalized * defaultSpeed;


		// reached target, go seek next one
		if (v.sqrMagnitude < 0.1f) {
			currentWaypointIndex++;
		}
	}



	
	// Update is called once per frame
	void FixedUpdate () {
		
		float time = Time.time;

		WalkToWaypoint();
		


		if (attackCooldownTimer.IsOffCooldown(time)) {
			attackCooldownTimer.Reset(time);
			Attack(time);
		}

		if (attackDurationTimer.IsOffCooldown(time)) {
			ktpAttack.SetActive(false);
		}
	}
}

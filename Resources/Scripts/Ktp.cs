using UnityEngine;
using System.Collections;

public class Ktp : MonoBehaviour {

	const float defaultSpeed = 3f;
	const float attackSpeed = 30f;

	Transform[] waypoints;
	public int currentWaypointIndex;

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
		attackCooldownTimer = new Timer(0.15f);
		attackDurationTimer = new Timer(0.1f);

		currentWaypointIndex = 0;
	}


	void Attack (Vector3 d) {

		d = d.normalized * attackSpeed;

		ktpAttack.GetComponent<Transform>().position = GetComponent<Transform>().position;
		ktpAttack.GetComponent<Rigidbody>().velocity = d;

		ktpAttack.SetActive(true);
		attackDurationTimer.Reset();
	}


	void OnTriggerEnter(Collider collider) {
    int l = collider.gameObject.layer;
    if (l == Helper.swarmLayer ||
    		l == Helper.obstacleLayer) {
			if (attackCooldownTimer.IsOffCooldown()) {
				attackCooldownTimer.Reset();

				Vector3 d = collider.gameObject.GetComponent<Transform>().position - GetComponent<Transform>().position;
				Attack (d);
			}
    }
	}


	void WalkToWaypoint () {

		if (currentWaypointIndex == waypoints.Length) {
			return;
		}

		Vector3 tp = waypoints[currentWaypointIndex].GetComponent<Transform>().position;
		Vector3 pp = GetComponent<Transform>().position;
		Vector3 d = tp - pp;
		Vector3 v = d.normalized * defaultSpeed;

		GetComponent<Rigidbody>().velocity = new Vector3(v.x, GetComponent<Rigidbody>().velocity.y, v.z);
		GetComponent<Rigidbody>().velocity = v;


		// reached target, go seek next one
		if (d.sqrMagnitude < 0.1f) {
			currentWaypointIndex++;
		}
	}



	
	// Update is called once per frame
	void FixedUpdate () {

		if (attackCooldownTimer.IsOffCooldown()) {
			WalkToWaypoint();
		}

		if (attackDurationTimer.IsOffCooldown()) {
			ktpAttack.SetActive(false);
		}
	}
}

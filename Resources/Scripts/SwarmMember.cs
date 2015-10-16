using UnityEngine;
using System.Collections;

public class SwarmMember : MonoBehaviour {

	const float defaultSpeed = 20f;
	GameObject Ktp;

	enum State { RunToward, Encircle, Tackle, Disabled}
	State currentState;


	Transform[] waypoints;

	// Use this for initialization
	void Start () {
		currentState = State.RunToward;
		
		Ktp = GameObject.Find("Ktp");

		waypoints = new Transform[6];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints[i] = GameObject.Find("Waypoints/Waypoint" + i).GetComponent<Transform>();
		}
	}



	void Tackle () {

	}
	

	void SeekTarget () {
		Vector3 v = Ktp.GetComponent<Transform>().position - GetComponent<Transform>().position;
		v = v.normalized * defaultSpeed;
		GetComponent<Rigidbody>().velocity = new Vector3(v.x, 0f, v.z);

		float y = 0f;
  	GetComponent<Transform>().eulerAngles = new Vector3(0f, y, 0f);
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (currentState == State.RunToward) {
			SeekTarget();
		}
	}
}

using UnityEngine;
using System.Collections;

public class SwarmMember : MonoBehaviour {

	bool isDisabled;

	const float defaultSpeed = 20f;
	GameObject Ktp;

	// Use this for initialization
	void Start () {
		isDisabled = false;
		Ktp = GameObject.Find("Ktp");
	}



	void Tackle () {

	}
	

	void SeekTarget () {

	}


	// Update is called once per frame
	void FixedUpdate () {
	
	}
}

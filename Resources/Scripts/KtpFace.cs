using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KtpFace : MonoBehaviour {

	GameObject mainCamera;
	GameObject ktp;

	Vector3 prevKtpP;

	public bool isFacingDown;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find("Main Camera");
		ktp = GameObject.Find("Ktp");
		prevKtpP = ktp.GetComponent<Transform>().position;
	}


	// Update is called once per frame
	void Update () {
		Vector3 v = ktp.GetComponent<Rigidbody>().velocity;

		if (v.sqrMagnitude < 0.2f) {
			return;
		}

		if (isFacingDown) {
			GetComponent<Canvas>().enabled = v.z > v.x;
		} else {
			GetComponent<Canvas>().enabled = v.z < v.x;
		}
		


		Vector3 p = ktp.GetComponent<Transform>().position;


		GetComponent<Transform>().position += (p - prevKtpP);


		prevKtpP = p;

	}
}
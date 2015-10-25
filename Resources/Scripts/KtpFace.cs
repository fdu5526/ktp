using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KtpFace : MonoBehaviour {

	GameObject mainCamera;
	GameObject ktp;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find("Main Camera");
		ktp = GameObject.Find("Ktp");
	}


	// Update is called once per frame
	void Update () {
		//GetComponent<Transform>().LookAt(-mainCamera.GetComponent<Transform>().position);

	}
}
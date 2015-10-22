using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBox : MonoBehaviour {

	GameObject camera;
	Transform parent;

	// Use this for initialization
	void Start () {

		camera = GameObject.Find("Main Camera");
		parent = GetComponent<Transform>().parent;
	}

	
	// Update is called once per frame
	void Update () {
		GetComponent<Transform>().LookAt(-camera.GetComponent<Transform>().position);
		GetComponent<Transform>().localPosition = new Vector3(0f, 5.69f, 0f);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ending : MonoBehaviour {

	float transparency;
	bool activated;

	// Use this for initialization
	void Start () {
		transparency = 0f;
		activated = false;
	}


	public void Activate () {
		activated = true;
		transparency = 0f;
		GetComponent<Image>().color = new Color(1f, 1f, 1f, transparency);
	}
	
	// Update is called once per frame
	void Update () {
		if (activated && transparency < 1f) {
			transparency += 0.01f;
			GetComponent<Image>().color = new Color(1f, 1f, 1f, transparency);
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ending : MonoBehaviour {

	Sprite s1, s2;
	Timer timer;

	bool isS1;

	// Use this for initialization
	void Start () {
		isS1 = true;
		s1 = Resources.Load<Sprite>("UI/ending1");
		s2 = Resources.Load<Sprite>("UI/ending2");
		timer = new Timer(0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (timer.IsOffCooldown()) {
			GetComponent<Image>().sprite = isS1 ? s2 : s1;
			isS1 = !isS1;
			timer.Reset();
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueBox : MonoBehaviour {

	GameObject mainCamera;
	GameObject text;
	Timer dialogueTimer;

	// Use this for initialization
	void Start () {
		dialogueTimer = new Timer(StartTime);
		dialogueTimer.Reset();


		dialogueTimer.Reset();
		
		mainCamera = GameObject.Find("Main Camera");
		text = GetComponent<Transform>().Find("Text").gameObject;
	}

	float StartTime { get { return UnityEngine.Random.Range(0f, 4f); } }
	float NewTalkTime { get { return UnityEngine.Random.Range(2f, 7f); } }


	public void Activate () {
		text.GetComponent<DialogueText>().Activate();
	}

	
	// Update is called once per frame
	void Update () {
		GetComponent<Transform>().LookAt(-mainCamera.GetComponent<Transform>().position);
		GetComponent<Transform>().localPosition = new Vector3(0f, 5.69f, 0f);


		if (dialogueTimer.IsOffCooldown()) {
			Activate();
			dialogueTimer = new Timer(NewTalkTime);
			dialogueTimer.Reset();
		}
	}
}
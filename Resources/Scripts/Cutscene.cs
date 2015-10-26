using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {
	
	string[][] texts;
	int currentChunkIndex;
	float duration = 2f;

	int currentTextIndex;

	public bool hasStarted;
	
	GameObject text;

	// Use this for initialization
	void Start () {
		currentChunkIndex = -1;
		text = GetComponent<Transform>().Find("Text").gameObject;
		hasStarted = false;


		texts = new string[4][];
		texts[0] = new string[]
							 { "hello 1",
	 							 "hello 2"};
	 	texts[1] = new string[]
							 { "",
	 							 ""};
	 	texts[2] = new string[]
							 { "",
	 							 ""};
	 	texts[3] = new string[]
							 { "",
	 							 ""};


		Invoke("Play", 1f);
	}


	public void Play () {

		GetComponent<Canvas>().enabled = true;

		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState != SwarmMember.State.Dead &&
					  sm.currentState != SwarmMember.State.Disabled) {
					sm.currentState = SwarmMember.State.Pause;
				}
			}
		}

		currentChunkIndex++;
		currentTextIndex = 0;
		for (int i = 0; i < texts[currentChunkIndex].Length; i++) {
			Invoke("NextText", duration * (float)i);
		}
	}


	void NextText () {
		text.GetComponent<Text>().text = texts[currentChunkIndex][currentTextIndex];
		currentTextIndex++;

		if (currentTextIndex == texts[currentChunkIndex].Length) {
			Invoke("EndCutscene", duration);
		}
	}

	void EndCutscene () {
		GetComponent<Canvas>().enabled = false;
		
		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState == SwarmMember.State.Pause) {
					sm.currentState = SwarmMember.State.RunToward;
				}
			}
		}
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}

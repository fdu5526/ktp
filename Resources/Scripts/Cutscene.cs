using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {
	
	string[][] texts;
	int currentChunkIndex;
	float duration = 2.5f;

	int currentTextIndex;

	bool ending;
	bool isPlaying;

	
	GameObject text;

	// Use this for initialization
	void Start () {
		currentChunkIndex = -1;
		text = GetComponent<Transform>().Find("Text").gameObject;
		ending = false;
		isPlaying = false;


		texts = new string[4][];
		texts[0] = new string[]
							 { "Here she comes!",
							 	 "What is going on?",
							 	 "KTP is here to take our water!",
	 							 "Be careful!",
	 							 "But there is only one of her.",
	 							 "Let's go get her!",
	 							 "We charge together!",
	 							 "Protect the water fountain!!!!"};
	 	texts[1] = new string[]
							 { "Oh no! She took our water!",
	 							 "Our precious water!",
	 							 "What are we going to do now?",
	 							 "Protect the next fountain!",
	 							 "Use the weight pillars! Knock them down!",
	 							 "Block the paths!",
	 							 "She can't move 100 ton weights!"};
	 	texts[2] = new string[]
							 { "NOOOO!!!",
	 							 "We only have 1 water fountain left!",
	 							 "Protect it at all costs!",
	 							 "This is our last resort!",
	 							 "Tackle the red explosive barrels!",
	 							 "We will take her down with us!",
	 							 "Protect our water at all costs!",
	 							 "Our lives for water!"
	 							};
	 	texts[3] = new string[]
							 { "Our water!",
	 							 "It is all gone!",
	 							 "...",
		 						 "...",
		 						 "..."};


	 	Invoke("Play", 0.1f);
	}


	public void Play () {

		if (currentChunkIndex == texts.Length - 1) {
			return;
		}

		isPlaying = true;

		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState != SwarmMember.State.Dead &&
					  sm.currentState != SwarmMember.State.Disabled) {
					sm.Pause();
				}
			}
		}

		GetComponent<Canvas>().enabled = true;
		GameObject.Find("Ktp").GetComponent<Ktp>().disabled = true;

		currentChunkIndex++;
		currentTextIndex = 0;
	}


	void NextText () {
		GetComponent<Transform>().Find("Start").gameObject.SetActive(false);
		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState != SwarmMember.State.Dead &&
					  sm.currentState != SwarmMember.State.Disabled) {
					sm.Pause();
				}
			}
		}

		string s = "Grunt #" + (int)UnityEngine.Random.Range(1,1000) + ": " + texts[currentChunkIndex][currentTextIndex];
		text.GetComponent<Text>().text = s;
		currentTextIndex++;

		if (currentTextIndex == texts[currentChunkIndex].Length) {
			if (currentChunkIndex == texts.Length - 1) {
				Ending();
			} else {
				EndCutscene();
			}
			
		}
	}

	void EndCutscene () {
		GetComponent<Canvas>().enabled = false;
		GameObject.Find("Ktp").GetComponent<Ktp>().disabled = false;
		isPlaying = false;

		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState == SwarmMember.State.Pause) {
					sm.Unpause();
				}
			}
		}
	}


	void Ending () {
		ending = true;
		GetComponent<Transform>().Find("Ending").gameObject.SetActive(true);
		Invoke("ExitApp", 55f);
		GetComponents<AudioSource>()[0].Play();

		AudioSource[] audios = GameObject.Find("Main Camera").GetComponents<AudioSource>();
		for (int i = 0; i < audios.Length; i++) {
			audios[i].Stop();
		}
	}


	void ExitApp () {
		Application.Quit();
	}


	void Update () {
		if (ending && 
				Input.anyKey) {
			ExitApp();
		}

		if (isPlaying && 
				Input.GetKeyDown("space")){
			NextText();
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {
	
	public bool isPraising;

	string[][] texts;
	int currentChunkIndex;
	float duration = 2.5f;

	int currentTextIndex;

	int deathCount;

	bool ending;
	bool isPlaying;

	
	GameObject text;
	GameObject praisers;

	// Use this for initialization
	void Start () {
		currentChunkIndex = -1;
		text = GetComponent<Transform>().Find("Text").gameObject;
		ending = false;
		isPlaying = false;
		isPraising = false;

		praisers = GameObject.Find("FinalSwarmMembers");
		praisers.SetActive(false);


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
	 							 "Maybe the 100 ton weights can stop her!"};
	 	texts[2] = new string[]
							 { "It's hopeless!!!",
	 							 "We only have 1 water fountain left!",
	 							 "Protect it at all costs!",
	 							 "Use our last resort!",
	 							 "Tackle the red explosive barrels!",
	 							 "We will take her down with us!",
	 							 "Our lives for water!"
	 							};
	 	texts[3] = new string[]
							 { "Is this it?",
							 	 "We stood no chance to begin with.",
							 	 "Even though we are defeated,",
							 	 "And our water taken,",
							 	 "It is...",
							 	 "so...",
							 	 "Beautiful.",
							 	 "Nothing compares to her strength." };

	 	Invoke("Play", 0.1f);
	}


	public void Play () {

		if (currentChunkIndex == texts.Length - 1) {
			return;
		}

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

		GetComponent<Transform>().Find("TextBack").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
		GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().enabled = true;
		GameObject.Find("Ktp").GetComponent<Ktp>().disabled = true;

		currentChunkIndex++;
		currentTextIndex = 0;
		isPlaying = true;

		if (currentChunkIndex != 0){
			NextText();
		}
		
	}


	void NextText () {
		if (currentTextIndex == texts[currentChunkIndex].Length) {
			if (currentChunkIndex == texts.Length - 1) {
				Ending();
			} else {
				EndCutscene();
			}
			return;
		}

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
	}

	void EndCutscene () {
		GetComponent<Transform>().Find("TextBack").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
		GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().enabled = false;

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

	public void SwitchToPraise () {
		AudioSource[] audios = GameObject.Find("Main Camera").GetComponents<AudioSource>();
		for (int i = 0; i < audios.Length; i++) {
			audios[i].Stop();
		}

		Invoke("PlayPraiseMusic", 2f);
		praisers.SetActive(true);

		GameObject.Find("FinalSwarmMembers").SetActive(true);

		isPraising = true;
		GameObject[] swarms = GameObject.FindGameObjectsWithTag("Swarm");
		foreach (GameObject g in swarms) {
			SwarmMember sm = g.GetComponent<SwarmMember>();
			if (sm != null) {
				if (sm.currentState != SwarmMember.State.Dead &&
					  sm.currentState != SwarmMember.State.Disabled &&
					  sm.currentState != SwarmMember.State.Awe &&
					  g.GetComponent<Player>() == null) {
					sm.SwitchToPraise();
				}
			}
		}
	}


	void PlayPraiseMusic () {
		GetComponents<AudioSource>()[0].Play();
	}


	public void IncreaseDeathCount () { 
		deathCount++;
		GetComponent<Transform>().Find("Counter").gameObject.GetComponent<Text>().text = "Deaths: "  + deathCount;
	}


	void Ending () {
		ending = true;
		GetComponent<Transform>().Find("Ending").gameObject.SetActive(true);
		GetComponent<Transform>().Find("Ending").gameObject.GetComponent<Ending>().Activate();
	}


	void Update () {
		if (isPlaying && 
				Input.GetKeyDown("space")){
			NextText();
		}

		/*
		if(Input.GetKeyDown("l")) {
			SwitchToPraise();
		}
		*/
	}
}

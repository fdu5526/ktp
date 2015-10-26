using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {
	
	string[][] texts;
	int currentChunkIndex;

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


		Play();
	}


	public void Play () {
		currentChunkIndex++;
		currentTextIndex = 0;
		for (int i = 0; i < texts[currentChunkIndex].Length; i++) {
			Invoke("NextText", 2f * (float)i);
		}
	}


	void NextText () {
		text.GetComponent<Text>().text = texts[currentChunkIndex][currentTextIndex];
		currentTextIndex++;
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}

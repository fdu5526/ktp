using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBox : MonoBehaviour {

	Text questText;

	// Use this for initialization
	void Start () {

		questText = this.gameObject.GetComponentInChildren<Text>();


		
	}
	
	void ChangeQuest(string s){
		questText.text = s;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}

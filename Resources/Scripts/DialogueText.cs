using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueText : MonoBehaviour {


	string[][] texts;
	GameObject Ktp;

	float transparency = 0f;
	float rate = 0.05f;
	float duration = 1f;
	Timer durationTimer;

	bool isIncreasing = true;
	bool activated = false;


	// Use this for initialization
	void Start () {
		Ktp = GameObject.Find("Ktp");

		texts = new string[3][];
		texts[0] = new string[]
							 { "YAHHHHH!!!", 
								 "To battle!", 
								 "To glorious combat!", 
								 "Protect our water!",
								 "I long for combat!",
								 "My life for water!",
								 "Long live the water!",
								 "We will pravail!",
	 							 "We will endure!",
	 							 "To Victory!",
	 							 "There is only one of her!",
	 							 "We can do this!"};
	 	texts[1] = new string[]
	 						 { "She's too strong!",
								 "We will never give up!",
								 "Don't give up!",
								 "We will stop you!",
								 "For the sublime!",
								 "We stand as one!",
								 "Do not falter!",
								 "For the future!",
								 "ATTACK!!!",
								 "CHARGE!!!",
								 "Yes we can!",
								 "Stop her!",
								 "We seek justice!",
								 "We fear no enemy!"};
	 	texts[2] = new string[]
	 					   { "Do not despair!",
								 "We must stop her!",
								 "Everyone charge!",
								 "Believe in each other!",
								 "Water awaits us!",
								 "For water!",
								 "We must hold!",
								 "She is too powerful!",
								 "We can't stop her!",
								 "Stand together!",
								 "We must prevail!",
								 "We must endure!",
								 "We must not falter!",
								 "Don't lose hope!",
								 "Stay strong!",
								 "I do not fear death!"};

		durationTimer = new Timer(duration);
		GetComponent<Text>().color = Color.clear;
	}


  protected int QuoteLevel {
    get {
      int w = Ktp.GetComponent<Ktp>().currentWaypointIndex;
      if (w <= 1) { return 0; }
      else if (w <= 3) { return 1; }
      else { return 2; }
    }
  }


	public void Activate () {
		transparency = 0f;
		isIncreasing = true;
		string[] quotes = texts[QuoteLevel];
		int i = UnityEngine.Random.Range(0, quotes.Length);
		GetComponent<Text>().text = quotes[i];
		activated = true;
	}

	
	// Update is called once per frame
	void Update () {

		if (!activated || !durationTimer.IsOffCooldown()) {
			return;
		}

		GetComponent<Text>().color = new Color(1f, 1f, 1f, transparency);

		if (isIncreasing) {
			transparency += rate;
		} else {
			transparency -= rate;
		}

		if (transparency >= 1f) {
			durationTimer.Reset();
			isIncreasing = false;
		} else if (transparency <= 0f) {
			GetComponent<Text>().color = Color.clear;
			activated = false;
		}


		
		
	}
}

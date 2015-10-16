using UnityEngine;
using System.Collections;

public class SwarmMember : MonoBehaviour {

	protected enum State { RunToward, Encircle, Tackle, Disabled}
	protected State currentState;

	protected const float defaultSpeed = 20f;
	protected const float sqrtRoot2 = 1.414f;
	protected GameObject Ktp;

	protected Timer hitStunTimer;

  // audios
  protected AudioSource[] audios;

	// Use this for initialization
	void Start () {
	
	}


	protected 	void Initialize () {
		hitStunTimer = new Timer(1f);
		audios = GetComponents<AudioSource>();
		Ktp = GameObject.Find("Ktp");
	}



	protected virtual void Tackle () {

	}


	float RandomFloat { 
		get { 
			float f = 2000f * UnityEngine.Random.Range(0.8f, 1.2f);

			return Helper.FiftyFifty ? f : -f; 
		} 
	}


	void OnCollisionEnter (Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpAttackLayer) {
      currentState = State.Disabled;
      Vector3 v = new Vector3(RandomFloat, RandomFloat, RandomFloat);
      Vector3 p = GetComponent<Transform>().position;
      p = new Vector3(p.x, 0f, p.z);
      GetComponent<Rigidbody>().AddForceAtPosition(v, p);

      audios[(int)UnityEngine.Random.Range(0, 3)].Play();

    } else if (currentState == State.Disabled && l == Helper.environmentLayer) {
    	audios[(int)UnityEngine.Random.Range(0, 2)].Play();
    }
  }

  void OnCollisionStay (Collision collision) {
  	int l = collision.gameObject.layer;
  	if (currentState == State.Disabled && l == Helper.groundLayer) { // no longer flying
    	audios[0].Play();
      currentState = State.RunToward;
      hitStunTimer.Reset(Time.time);
    }
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}

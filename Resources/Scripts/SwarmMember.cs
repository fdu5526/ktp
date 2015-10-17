using UnityEngine;
using System.Collections;

public class SwarmMember : MonoBehaviour {

	protected enum State { RunToward, Encircle, Tackle, Disabled}
	protected State currentState;

	protected const float defaultSpeed = 15f;
	protected const float sqrtRoot2 = 1.414f;
	protected GameObject Ktp;


	protected const float tackleSpeed = 30f;
	protected Vector3 TackleDirection;
	protected Timer tackleDurationTimer;
	protected Timer tackleCooldownTimer;

	protected Timer hitGroundTimer;
	protected Timer hitStunTimer;

  // audios
  protected AudioSource[] audios;


	protected void Initialize () {
		tackleDurationTimer = new Timer(0.1f);
		tackleCooldownTimer = new Timer(1f);
		hitStunTimer = new Timer(1f);
		hitGroundTimer = new Timer(2f);
		audios = GetComponents<AudioSource>();
		Ktp = GameObject.Find("Ktp");
	}


	protected virtual void Tackle () { }


	float RandomFloat { 
		get { 
			float f = 1000f * UnityEngine.Random.Range(0.8f, 1.2f);

			return Helper.FiftyFifty ? f : -f; 
		} 
	}


	void OnCollisionEnter (Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpAttackLayer) {
      currentState = State.Disabled;
      Vector3 v = new Vector3(RandomFloat, RandomFloat, RandomFloat); 
      //Vector3 v = Vector3.zero; // TODO

      Vector3 p = GetComponent<Transform>().position;
      p = new Vector3(p.x, 0f, p.z);
      GetComponent<Rigidbody>().AddForceAtPosition(v, p);

      audios[(int)UnityEngine.Random.Range(0, 3)].Play();
      hitGroundTimer.Reset();

    } else if (currentState == State.Disabled && 
    					 (l == Helper.environmentLayer || l == Helper.groundLayer)) {
    	audios[(int)UnityEngine.Random.Range(0, 2)].Play();
    }
  }

  void OnCollisionStay (Collision collision) {
  	int l = collision.gameObject.layer;
  	if (currentState == State.Disabled && 
  			hitGroundTimer.IsOffCooldown() && 
  			l == Helper.groundLayer) { // no longer flying
      currentState = State.RunToward;
      hitStunTimer.Reset();
    }
  }
}

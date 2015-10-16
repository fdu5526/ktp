using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : MonoBehaviour 
{

  // can we do things
  bool isDisabled;
  Timer hitStunTimer;

  // player movement variables
  const float defaultSpeed = 20f;
  const float sqrtRoot2 = 1.414f;
  float yRotation;
  bool isWalking;

  // stores player input
  string[] wasdStrings = {"w", "a", "s", "d"};
  bool[] wasdInput;
  
  // audios
  AudioSource[] audios;

///////////////////////////////////////////////////////////////////////////////////////////  

  // Use this for initialization
	void Start () {
    isDisabled = false;
    isWalking = false;
		audios = GetComponents<AudioSource>();
    wasdInput = new bool[wasdStrings.Length];

    hitStunTimer = new Timer(1f);
	}


	// get player keyboard input, do things
	void CheckMovement() {
		Vector3 v = GetComponent<Rigidbody>().velocity;
    isWalking = true;

    float vx = 0f;
    float vz = 0f;


    if (wasdInput[0]) {
      if (wasdInput[1]) {
        vz = defaultSpeed;
        yRotation = 0f;
      }
      else if (wasdInput[3]) {
        vx = defaultSpeed;
        yRotation = 90f;
      }
      else {
        vx = defaultSpeed/sqrtRoot2;
        vz = defaultSpeed/sqrtRoot2;
        yRotation = 45f;
      }
    } else if (wasdInput[2]) {
      if (wasdInput[1]) {
        vx = -defaultSpeed;
        yRotation = 270f;
      }
      else if (wasdInput[3]) {
        vz = -defaultSpeed;
        yRotation = 180f;
      } else {
        vx = -defaultSpeed/sqrtRoot2;
        vz = -defaultSpeed/sqrtRoot2;
        yRotation = 225f;
      }
    } else if (wasdInput[1]) {
      vx = -defaultSpeed/sqrtRoot2;
      vz = defaultSpeed/sqrtRoot2;
      yRotation = 315f;
    }
    else if (wasdInput[3]) {
      vx = defaultSpeed/sqrtRoot2;
      vz = -defaultSpeed/sqrtRoot2;
      yRotation = 135f;
    } else {
      isWalking = false;
    }
    GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Lerp(vx, v.x, 0.5f), 
                                                     v.y,
                                                     Mathf.Lerp(vz, v.z, 0.5f));

  	// rotate player towards correct direction
    float y = GetComponent<Transform>().eulerAngles.y;
  	GetComponent<Transform>().eulerAngles = new Vector3(0f, Mathf.Lerp(yRotation, y, 0.1f), 0f);
		
	}

  // play stepping sounds when moving 
  void StepSounds () {

    if (isWalking) {
      
    } else {

    }
  }


  void OnCollisionEnter(Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpLayer) {
      isDisabled = true;
      GetComponent<Rigidbody>().AddForce(Vector3.up * 1000f);
    } else if (isDisabled && l == Helper.groundLayer) {
      isDisabled = false;
      hitStunTimer.Reset(Time.time);
    }
  }



  // do physics stuff
  void FixedUpdate () {

    if (!isDisabled && hitStunTimer.IsOffCooldown(Time.time)) {
      CheckMovement();
      StepSounds();
    }
  }	

	// get player input
	void Update () {
    for (int i = 0; i < wasdStrings.Length; i++) {
      wasdInput[i] = Input.GetKey(wasdStrings[i]);
    }
	}
}

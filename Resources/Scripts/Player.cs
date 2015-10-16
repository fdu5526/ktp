using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : SwarmMember 
{

  // can we do things
  bool isDisabled;

  // player movement variables
  const float defaultSpeed = 20f;
  float yRotation;
  bool isWalking;

  // stores player input
  string[] wasdStrings = {"w", "a", "s", "d"};
  bool[] wasdInput;
  

///////////////////////////////////////////////////////////////////////////////////////////  

  // Use this for initialization
	void Start () {
    isDisabled = false;
    isWalking = false;
    wasdInput = new bool[wasdStrings.Length];

    base.Initialize();
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

    if (currentState == State.RunToward || currentState == State.Encircle) {
      
    } else {

    }
  }




  // do physics stuff
  void FixedUpdate () {

    if (currentState != State.Disabled && hitStunTimer.IsOffCooldown(Time.time)) {
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

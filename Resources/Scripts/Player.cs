using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : SwarmMember 
{

  // can we do things
  bool isDisabled;

  // player movement variables
  float yRotation;
  bool isWalking;

  // stores player input
  string[] inputStrings = {"w", "a", "s", "d", "space"};
  bool[] inputs;
  

///////////////////////////////////////////////////////////////////////////////////////////  

  // Use this for initialization
	void Start () {
    isDisabled = false;
    isWalking = false;
    inputs = new bool[inputStrings.Length];

    base.Initialize();
	}


	// get player keyboard input, do things
	void CheckMovement() {
		Vector3 v = GetComponent<Rigidbody>().velocity;
    isWalking = true;

    float vx = 0f;
    float vz = 0f;


    if (inputs[0]) {
      if (inputs[1]) {
        vz = defaultSpeed;
        yRotation = 0f;
      }
      else if (inputs[3]) {
        vx = defaultSpeed;
        yRotation = 90f;
      }
      else {
        vx = defaultSpeed/sqrtRoot2;
        vz = defaultSpeed/sqrtRoot2;
        yRotation = 45f;
      }
    } else if (inputs[2]) {
      if (inputs[1]) {
        vx = -defaultSpeed;
        yRotation = 270f;
      }
      else if (inputs[3]) {
        vz = -defaultSpeed;
        yRotation = 180f;
      } else {
        vx = -defaultSpeed/sqrtRoot2;
        vz = -defaultSpeed/sqrtRoot2;
        yRotation = 225f;
      }
    } else if (inputs[1]) {
      vx = -defaultSpeed/sqrtRoot2;
      vz = defaultSpeed/sqrtRoot2;
      yRotation = 315f;
    }
    else if (inputs[3]) {
      vx = defaultSpeed/sqrtRoot2;
      vz = -defaultSpeed/sqrtRoot2;
      yRotation = 135f;
    } else {
      isWalking = false;
    }
    v = new Vector3(Mathf.Lerp(vx, v.x, 0.5f), 
                    v.y,
                    Mathf.Lerp(vz, v.z, 0.5f));
    GetComponent<Rigidbody>().velocity = v;
    if (v.sqrMagnitude > 0f) {
      TackleDirection = v.normalized;
    }

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


  protected override void Tackle () {
    GetComponent<Rigidbody>().velocity = TackleDirection * tackleSpeed;
    currentState = State.Tackle;
  }


  void CheckTackle () {
    if (inputs[4] && 
        tackleCooldownTimer.IsOffCooldown()) {
      Tackle();
      tackleCooldownTimer.Reset();
      tackleDurationTimer.Reset();
    } else if (!tackleDurationTimer.IsOffCooldown()) {
      Tackle();
    } else if (currentState == State.Tackle) {
      hitStunTimer.Reset();
      currentState = State.RunToward;
    }
  }


  // do physics stuff
  void FixedUpdate () {

    if (currentState != State.Disabled && hitStunTimer.IsOffCooldown()) {
      
      if (currentState == State.RunToward) {
        CheckMovement();
      }
      
      CheckTackle();
      StepSounds();
    }
  }	

	// get player input
	void Update () {
    for (int i = 0; i < inputStrings.Length; i++) {
      inputs[i] = Input.GetKey(inputStrings[i]);
    }
	}
}

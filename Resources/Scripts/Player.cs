using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : SwarmMember 
{

  // player movement variables
  float yRotation;
  bool isWalking;

  // stores player input
  string[] inputStrings = {"w", "a", "s", "d", "space"};
  bool[] inputs;
  

///////////////////////////////////////////////////////////////////////////////////////////  

  // Use this for initialization
	void Start () {
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

    if (isWalking) {
      animator.SetInteger("currentState", (int)State.RunToward);
    } else {
      animator.SetInteger("currentState", 1);
    }

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


  void CheckTackle () {
    if (inputs[4] && 
        tackleCooldownTimer.IsOffCooldown()) {
      Tackle();
    } else if (currentState == State.Tackle) {
      attackStunTimer.Reset();
      currentState = State.RunToward;
    }
  }


  protected override void Respawn () {
    int w = NearestRespawnPoint();
    GameObject g = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Player"));

    Vector2 r = UnityEngine.Random.insideUnitCircle * 5f;
    g.GetComponent<Transform>().position = waypoints[w].position + new Vector3(r.x, 0f, r.y);

    this.gameObject.name = "ded";
    GameObject.Find("Main Camera").GetComponent<Camera>().ReloadPlayerTransform();
    Destroy(this);
  }


  // do physics stuff
  void FixedUpdate () {
    if (currentState == State.Dead) {
      return;
    }

    if (currentState != State.Disabled && attackStunTimer.IsOffCooldown()) {
      if (currentState == State.RunToward) {
        CheckMovement();
      }
      CheckTackle();
      StepSounds();
    } else if (currentState == State.Disabled) {
      if (flailTimer.IsOffCooldown()) {
        animator.SetInteger("currentState", (int)State.Dead);
      }
      if (respawnTimer.IsOffCooldown())
      {
        currentState = State.Dead;
        Respawn();
      }
    }
  }	

	// get player input
	void Update () {
    for (int i = 0; i < inputStrings.Length; i++) {
      inputs[i] = Input.GetKey(inputStrings[i]);
    }
	}
}

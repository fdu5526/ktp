using UnityEngine;
using System.Collections;

public class SwarmMember : MonoBehaviour {

	public enum State { RunToward, Encircle, Tackle, Disabled, Dead, Pause, Awe}
	public State currentState;

	protected const float defaultSpeed = 10f;
	protected const float sqrtRoot2 = 1.414f;
	protected GameObject Ktp;
  protected Transform[] waypoints;


	protected const float tackleSpeed = 40f;
	protected Vector3 TackleDirection;
	protected Timer tackleDurationTimer;
	protected Timer tackleCooldownTimer;

  protected Timer flailTimer;
	protected Timer respawnTimer;
	protected Timer attackStunTimer;

  protected Cutscene cutscene;

  // audios
  protected Animator animator;
  protected AudioSource[] audios;


	protected void Initialize () {
		tackleDurationTimer = new Timer(0.3f);
		tackleCooldownTimer = new Timer(1.5f);
		attackStunTimer = new Timer(1f);
    flailTimer = new Timer(1.5f);
		respawnTimer = new Timer(3f);
		audios = GetComponents<AudioSource>();
		Ktp = GameObject.Find("Ktp");
    waypoints = new Transform[7];
    for (int i = 0; i < waypoints.Length; i++) {
      waypoints[i] = GameObject.Find("Waypoints/Waypoint" + i).GetComponent<Transform>();
    }
    animator = GetComponent<Animator>();
    cutscene = GameObject.Find("UI").GetComponent<Cutscene>();
	}

  protected void Tackle () {
    GetComponent<Rigidbody>().velocity = TackleDirection * tackleSpeed;
    currentState = State.Tackle;
    tackleCooldownTimer.Reset();
    tackleDurationTimer.Reset();
    audios[3].Play();
    animator.SetInteger("currentState", (int)State.Tackle);
  }

  protected int NearestRespawnPoint {
    get {
      int w = Ktp.GetComponent<Ktp>().currentWaypointIndex;
      if (w <= 1) { return 1; }
      else if (w <= 3) { return 3; }
      else { return 6; }
    }
  }

  protected virtual void Respawn () { }

	float RandomFloat { 
		get { 
			float f = 1500f * UnityEngine.Random.Range(0.8f, 1.2f);

			return Helper.FiftyFifty ? f : -f; 
		} 
	}


  public void SwitchToPraise () {
    currentState = State.Awe;
    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    GetComponent<Transform>().eulerAngles = Vector3.zero;
    animator.SetInteger("currentState", 1);
  }
  
  public void Pause () {
    currentState = State.Pause;
    GetComponent<Rigidbody>().velocity = Vector3.zero;
    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    GetComponent<Transform>().eulerAngles = Vector3.zero;
    animator.SetInteger("currentState", 1);
    if (GetComponent<Transform>().Find("Canvas") != null) {
      GetComponent<Transform>().Find("Canvas").gameObject.SetActive(false);
    }
  }


  public void Unpause () {
    currentState = State.RunToward;
    animator.SetInteger("currentState", (int)State.RunToward);
    GetComponent<Transform>().Find("Canvas").gameObject.SetActive(true);
  }

	void OnCollisionEnter (Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpAttackLayer ||
        l == Helper.explosionLayer) {

      if (currentState != State.Disabled &&
          currentState != State.Dead) {
        Invoke("Respawn", 3f);
        cutscene.IncreaseDeathCount();
      }
      
      currentState = State.Disabled;
      Vector3 v = new Vector3(RandomFloat / 2f, RandomFloat, RandomFloat / 2f);

      Vector3 p = GetComponent<Transform>().position;
      p = new Vector3(p.x, 0f, p.z);
      GetComponent<Rigidbody>().AddForceAtPosition(v, p);

      audios[(int)UnityEngine.Random.Range(0, 3)].Play();
      if (audios != null) {
        
      }

      if (respawnTimer != null) {
        respawnTimer.Reset();
      }
      if (flailTimer != null) {
        flailTimer.Reset();
      }

      if (animator != null) {
        animator.SetInteger("currentState", (int)State.Disabled);
      }

      int i = (int)UnityEngine.Random.Range(0f,7f);
      if (i < 5 && audios != null) {
        AudioClip a = (AudioClip)MonoBehaviour.Instantiate(Resources.Load("Sounds/Deaths/male" + i));
        audios[0].clip = a;
        audios[0].Play();
      }

      if (GetComponent<Transform>().Find("Canvas") != null) {
        if (GetComponent<Transform>().Find("Canvas").gameObject != null) {
          GetComponent<Transform>().Find("Canvas").gameObject.SetActive(false);
        }
      }
      

    } else if (currentState == State.Disabled && 
               audios != null && 
    					 (l == Helper.environmentLayer || l == Helper.groundLayer)) {
    	audios[(int)UnityEngine.Random.Range(0, 2)].Play();
    } else if (l == Helper.swarmLayer) {
      SwarmMember s = collision.gameObject.GetComponent<SwarmMember>();
      if (attackStunTimer != null && 
          s != null &&
          s.currentState == State.Disabled) {
        attackStunTimer.Reset();
        audios[(int)UnityEngine.Random.Range(0, 2)].Play();
      }
    }
  }
}

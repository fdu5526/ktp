using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour {

	private float prevOpenTime;
	private Player playerScript;
	private const float suspicionIncreaseUponCollision = 5f;

	// Use this for initialization
	void Start () {
		prevOpenTime = -100f;
		playerScript = (GameObject.Find ("Player")).GetComponent<Player>();
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name.Equals("Player") && Time.time - prevOpenTime > 1f)
		{
			GetComponent<AudioSource>().Play();
			prevOpenTime = Time.time;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

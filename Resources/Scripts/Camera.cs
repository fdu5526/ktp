using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	// the player's transform
	private Transform playerTransform;
	private Transform ktpTransform;
	private const float y = 100.2f;
	AudioSource[] audios;
	float ScarySoundDistance = 30f;

	// Use this for initialization
	void Start () 
	{
		// the player's gameobject must be named Player
		playerTransform = GameObject.Find ("Player").transform;
		ktpTransform = GameObject.Find ("Ktp").transform;
		audios = GetComponents<AudioSource>();


		//TODO
		for (int i = 0; i < audios.Length; i++) {
			//audios[i].volume = 0f;
		}
	}


	public void ReloadPlayerTransform () {
		GameObject g = GameObject.Find ("Player(Clone)");
		if (g.GetComponent<SwarmMember>().currentState != SwarmMember.State.Dead) {
			playerTransform = GameObject.Find ("Player(Clone)").transform;
		}
		
	}
	

	void ScarySound () {
		Vector3 d = ktpTransform.position - playerTransform.position;
		audios[1].volume = Mathf.Max(ScarySoundDistance - d.magnitude, 0f) / (8f * ScarySoundDistance);
	}


	// Update is called once per frame
	void Update () 
	{
		int w = ktpTransform.gameObject.GetComponent<Ktp>().currentWaypointIndex;
		if (!audios[3].isPlaying && 
				w == 4) {
				audios[0].Stop();
				audios[1].Stop();
				audios[2].Stop();
				audios[3].Play();
		}
		if (!audios[2].isPlaying && 
				w == 2) {
				audios[0].Stop();
				audios[1].Stop();
				audios[2].Play();
		} else if (w < 2) {
			ScarySound();
		}
		
		float x = playerTransform.position.x - 88.5f;
		float z = playerTransform.position.z - 71.6f;
		transform.position = new Vector3(x,y,z) * 0.1f + 0.9f * transform.position;
	}
}

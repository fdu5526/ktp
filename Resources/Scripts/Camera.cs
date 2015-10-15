using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	// the player's transform
	private Transform playerTransform;
	private const float y = 100.2f;

	// Use this for initialization
	void Start () 
	{
		// the player's gameobject must be named Player
		playerTransform = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x = playerTransform.position.x - 88.5f;
		float z = playerTransform.position.z - 71.6f;
		transform.position = new Vector3(x,y,z);
	}
}

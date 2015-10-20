using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {


  AudioSource[] audios;

  float RandomFloat { 
    get { 
      float f = 1500f * UnityEngine.Random.Range(0.8f, 1.2f);

      return Helper.FiftyFifty ? f : -f; 
    } 
  }

	void OnCollisionEnter (Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpAttackLayer) {
      Vector3 v = new Vector3(RandomFloat / 2f, RandomFloat, RandomFloat / 2f);

      Vector3 p = GetComponent<Transform>().position;
      p = new Vector3(p.x, 0f, p.z);
      GetComponent<Rigidbody>().AddForceAtPosition(v, p);

      if (audios != null) {
        audios[(int)UnityEngine.Random.Range(0, 3)].Play();
      }
    }
  }
}

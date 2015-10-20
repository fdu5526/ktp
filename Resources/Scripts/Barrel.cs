using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {


  AudioSource[] audios;
  GameObject explosion;

  void Start () {
    explosion = GetComponent<Transform>().Find("Explosion").gameObject;
    audios = GetComponents<AudioSource>();
  }

  float RandomFloat { 
    get { 
      float f = 1500f * UnityEngine.Random.Range(0.8f, 1.2f);

      return Helper.FiftyFifty ? f : -f; 
    } 
  }


  void StopExplosion () {
    explosion.SetActive(false);
  }


  void Explode () {
    GetComponent<CapsuleCollider>().enabled = false;
    GetComponent<MeshRenderer>().enabled = false;
    explosion.GetComponent<Transform>().localPosition = new Vector3(0f, -5.83f, 0f);
    explosion.GetComponent<Rigidbody>().velocity = new Vector3(0f, 20f, 0f);
    Invoke("StopExplosion", 0.5f);
    audios[0].Play();
  }

	void OnCollisionEnter (Collision collision) {
    int l = collision.gameObject.layer;
    if (l == Helper.ktpAttackLayer ||
        l == Helper.explosionLayer) {
      Explode();
    } else if (l == Helper.swarmLayer) {
      SwarmMember s = collision.gameObject.GetComponent<SwarmMember>();
      if (s != null && 
          s.currentState == SwarmMember.State.Tackle) {
        Explode();
      }
    }
  }
}

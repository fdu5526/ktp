using UnityEngine;
using System.Collections;

public class Fountain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	public void DryOut () {

		GameObject polysurface = GetComponent<Transform>().Find("big").gameObject;

		Renderer r = polysurface.GetComponent<Renderer>();
    Material[] materials = r.materials;
    materials[1] = Resources.Load("Materials/10") as Material;
    r.materials = materials;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

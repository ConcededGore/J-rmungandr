using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletCorrection : NetworkBehaviour {
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.right = rb.velocity.normalized;
		Vector3 newScale = transform.localScale;
		newScale.x = rb.velocity.magnitude/20.0f+1;
		transform.localScale = newScale;
	}
}

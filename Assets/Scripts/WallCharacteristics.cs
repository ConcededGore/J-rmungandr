using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCharacteristics : MonoBehaviour {
	public float width;
	public float height;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		BoxCollider2D box = GetComponent<BoxCollider2D> ();
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		box.size = new Vector2 (width, height);
		sprite.size = new Vector2 (width, height);
	}

	void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height, 1));
	}
}

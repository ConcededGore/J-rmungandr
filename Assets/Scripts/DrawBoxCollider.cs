using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoxCollider : MonoBehaviour {

	BoxCollider2D bc;

	void OnDrawGizmos() {
		bc = this.GetComponent<BoxCollider2D> ();
		Gizmos.DrawWireCube (transform.position, bc.size);
	}
}
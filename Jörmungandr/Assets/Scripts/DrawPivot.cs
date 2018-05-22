using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPivot : MonoBehaviour {

	public Transform parentTrans;
	public bool isFlipped = false;

	void Start() {
		parentTrans = transform.parent.transform;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, .1f);
	}

	// Update is called once per frame
	void Update () {
		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > parentTrans.position.x) {
			isFlipped = true;
			transform.localPosition = new Vector3(.5f, .25f, -1f);
			transform.GetChild(0).GetComponent<SpriteRenderer> ().flipY = false;
			transform.GetChild (0).transform.GetChild (0).transform.localPosition = new Vector3(5.44f, 1.93f, -3f);
		} else {
			isFlipped = false;
			transform.localPosition = new Vector3(-.5f, .25f, -1f);
			transform.GetChild(0).GetComponent<SpriteRenderer> ().flipY = true;
			transform.GetChild (0).transform.GetChild (0).transform.localPosition = new Vector3(5.44f, -1.93f, -3f);
		} 
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookAtMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!transform.parent.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer) {
			return;
		}

		Vector2 mousePos;
		float angle;
		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 toMouse = mousePos - new Vector2(transform.position.x, transform.position.y);
		angle = Mathf.Atan2 (toMouse.x, toMouse.y);
		Vector3 rotation = transform.rotation.eulerAngles;
		rotation.z = 90 - angle * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (rotation);
	}
}

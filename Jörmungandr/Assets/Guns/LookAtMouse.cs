using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookAtMouse : MonoBehaviour {

	public bool mirrored = false;

	private float initRecoil = 0;
	private float recoil = 0;
	private float recovTime = 0; //in seconds

	private DrawPivot _drawPivot;

	// Use this for initialization
	void Start () {
		_drawPivot = GetComponent<DrawPivot> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!transform.parent.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer || transform.parent.gameObject.GetComponent<PlayerController>().IsDead) {
			return;
		}

		Vector2 mousePos;
		float angle;
		if (Camera.main.ScreenToWorldPoint (Input.mousePosition).x < transform.parent.gameObject.transform.position.x) {
			transform.parent.gameObject.transform.localRotation.eulerAngles.Set(0, 180, 0);
		} else {
			transform.parent.gameObject.transform.localRotation.eulerAngles.Set(0, 0, 0);
		}
		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 toMouse = mousePos - new Vector2(transform.position.x, transform.position.y);
		angle = Mathf.Atan2 (toMouse.x, toMouse.y);
		angle += recoil;
		Vector3 rotation = transform.rotation.eulerAngles;
		rotation.z = 90 - angle * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (rotation);

		if (_drawPivot != null) {
			initRecoil = -1 * Mathf.Abs (initRecoil);
			if (_drawPivot.isFlipped) {
				if (recoil < -.1f) {
					recoil -= CalcRecoilRecov () * Mathf.Rad2Deg;
				}
				if (recoil > -.1f && recoil < .1f) {
					recoil = 0;
					initRecoil = 0;
					recovTime = 0;
				}
			} else {
				initRecoil = Mathf.Abs (initRecoil);
				if (recoil < -.1f) {
					recoil -= CalcRecoilRecov () * Mathf.Rad2Deg;
				}
				if (recoil > -.1f && recoil < .1f) {
					recoil = 0;
					initRecoil = 0;
					recovTime = 0;
				}
			}
		}
	}

	private float CalcRecoilRecov() {
		return (Time.deltaTime / recovTime) * initRecoil;
	}

	public void Recoil(float degrees, float recovery) {
		recoil += degrees * Mathf.Deg2Rad;
		initRecoil += recoil;
		recovTime = recovery;
	}
}

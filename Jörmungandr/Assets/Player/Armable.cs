using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Armable : NetworkBehaviour {

	public WeaponBase weapon;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) {
			return;
		}
		// runs on client	
		if (weapon.autoFire) {
			if (Input.GetMouseButton (0)) {
				CmdFireAuto ();
			} else {
				weapon.lastFired = weapon.lastFired + weapon.firerate < Time.time ? Time.time - weapon.firerate : weapon.lastFired;
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			CmdFire ();
		} else {
			weapon.lastFired = weapon.lastFired + weapon.firerate*1.05f < Time.time ? Time.time - weapon.firerate*1.05f : weapon.lastFired;
		}

	}

	// Commands run on server
	[Command]
	void CmdFireAuto() {
		weapon.AutoFire ();
	}

	[Command]
	void CmdFire() {
		weapon.SingleFire ();
	}
}

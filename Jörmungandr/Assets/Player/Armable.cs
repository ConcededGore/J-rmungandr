using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Armable : NetworkBehaviour {

	public bool timerFlag = false;

	private float timer = 0;

	private int idNum = 0;

	public WeaponBase weapon;

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (!isLocalPlayer) {
			return;
		}
		// runs on client	
		if (weapon.autoFire) {
			if (Input.GetMouseButton (0) && (weapon.currAmmo > 0 || weapon.infiniteAmmo)) {
				while (timer >= weapon.firerate) {
					CmdFireAuto (GetComponent<NetworkIdentity>().netId);
					timer -= weapon.firerate;
				}
			} else {
				if (timer >= weapon.firerate) {
					timer = weapon.firerate;
				}
			}
		} else {

			if (Input.GetMouseButtonDown (0)) {
				CmdFire ();
			} else {
				//weapon.lastFired = Time.time;
				//weapon.lastFired = weapon.lastFired + weapon.firerate < Time.time ? Time.time - weapon.firerate : weapon.lastFired;
			}
		}

		if (Input.GetKeyDown(KeyCode.R) && !Input.GetMouseButton(0)) {
			weapon.Reload();
		}

	}

	// Commands run on server
	[Command]
	void CmdFireAuto(NetworkInstanceId netId) {
		weapon.AutoFire (netId);
	}

	[Command]
	void CmdFire() {
		weapon.SingleFire ();
	}
}

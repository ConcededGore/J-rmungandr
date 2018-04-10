using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WeaponBase : NetworkBehaviour {

	public GameObject bullet;
	public GameObject SpawnPos;

	public float firerate  = .25f; // Measured in bullets per second
	public float reloadTime = 1; // In seconds
	public float bulletLifetime = 1; // ButtSecs
	public float bulletVelocity = .5f; // Meters per second
	public float fireField = 10f; //Measured in degrees
	//public float recoil; Not sure if we'll use this

	public int ammoCount; // Number of bullets in a full clip
	//public int damage; Also not sure if we'll use this either

	public bool autoFire = true; // True for fullauto, false for semi


	private float timer = 0;

	private float lastFired;

	// Use this for initialization
	void Start () {
		lastFired = -firerate;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isLocalPlayer) {
			return;
		}
		// runs on client	
		if (autoFire) {
			if (Input.GetMouseButton (0)) {
				//CmdFireAuto ();
			} else {
				lastFired = lastFired + firerate < Time.time ? Time.time - firerate : lastFired;
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			//CmdFire ();
		} else {
			lastFired = lastFired + firerate*1.05f < Time.time ? Time.time - firerate*1.05f : lastFired;
		}

	}
}

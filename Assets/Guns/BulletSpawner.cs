using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletSpawner : NetworkBehaviour {

	public GameObject bullet;
	public GameObject SpawnPos;

	public float firerate  = 1; // Measured in bullets per second
	public float reloadTime = 1; // In seconds
	public float bulletLifetime = 1; // ButtSecs
	public float bulletVelocity = .5f; // Meters per second
	public float fireFeild = 10f; //Measured in degrees
	//public float recoil; Not sure if we'll use this

	public int ammoCount; // Number of bullets in a full clip
	//public int damage; Also not sure if we'll use this either

	public bool autoFire; // True for fullauto, false for semi


	private float timer = 0;
	private float lastFired;

	// Use this for initialization
	void Start () {
		firerate  = .1f;
		reloadTime = 1;
		bulletLifetime = 1;
		bulletVelocity = 10f;
		fireFeild = 1f;
		timer = 0;
		lastFired = 0;
		autoFire = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) {
			return;
		}
			
		CmdFire ();
	}

	[Command]
	void CmdFire() {
		if (autoFire) {
			if (Input.GetMouseButton (0)) {
				while (Time.time > lastFired + firerate) {
					lastFired += firerate;
					GameObject newBullet = Instantiate<GameObject> (bullet, SpawnPos.transform.position, SpawnPos.transform.rotation);
					Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D> ();
					rb.velocity = GetComponent<Rigidbody2D> ().velocity;
					Vector2 forceOnBullet = SpawnPos.transform.right;
					float theta = Random.Range (-fireFeild / 2f, fireFeild / 2f) * Mathf.Deg2Rad;
					Vector2 rotated = new Vector2 (Mathf.Cos (theta) * forceOnBullet.x - Mathf.Sin (theta) * forceOnBullet.y, Mathf.Sin (theta) * forceOnBullet.x + Mathf.Cos (theta) * forceOnBullet.y);
					rb.AddForce (rotated * bulletVelocity, ForceMode2D.Impulse);
					NetworkServer.Spawn (newBullet);
					Destroy (newBullet, bulletLifetime);
				}
			} else {
				lastFired = lastFired + firerate < Time.time ? Time.time - firerate : firerate;
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
				timer += Time.deltaTime;
				if (lastFired + firerate < timer) {
					lastFired = timer;
					GameObject newBullet = Instantiate<GameObject> (bullet, SpawnPos.transform.position, SpawnPos.transform.rotation);
					Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D> ();
					rb.velocity = GetComponent<Rigidbody2D> ().velocity;
					Vector2 forceOnBullet = SpawnPos.transform.right;
					float theta = Random.Range (-fireFeild / 2, fireFeild / 2)*Mathf.Deg2Rad;
					Vector2 rotated = new Vector2 (Mathf.Cos(theta) * forceOnBullet.x - Mathf.Sin(theta) * forceOnBullet.y, Mathf.Sin(theta) * forceOnBullet.x + Mathf.Sin(theta) * forceOnBullet.y);
					rb.AddForce (rotated*bulletVelocity, ForceMode2D.Impulse);
					NetworkServer.Spawn (newBullet);
					Destroy (newBullet, bulletLifetime);
				}
			}
		}
	}

}

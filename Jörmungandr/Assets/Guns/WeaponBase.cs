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

	public float lastFired;

	// Use this for initialization
	void Start () {
		lastFired = -firerate;
	}

	public virtual void AutoFire() {
		while (Time.time > lastFired + firerate) {
			lastFired += firerate;
			GameObject newBullet = Instantiate<GameObject> (bullet, SpawnPos.transform.position, SpawnPos.transform.rotation);
			Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D> ();
			rb.velocity = GetComponent<Rigidbody2D> ().velocity;
			Vector2 forceOnBullet = SpawnPos.transform.right;
			float theta = Random.Range (-fireField / 2f, fireField / 2f) * Mathf.Deg2Rad;
			Vector2 rotated = new Vector2 (Mathf.Cos (theta) * forceOnBullet.x - Mathf.Sin (theta) * forceOnBullet.y, Mathf.Sin (theta) * forceOnBullet.x + Mathf.Cos (theta) * forceOnBullet.y);
			rb.AddForce (rotated * bulletVelocity, ForceMode2D.Impulse);
			NetworkServer.Spawn (newBullet);
			Destroy (newBullet, bulletLifetime);
		}
	}

	public virtual void SingleFire() {
		if (lastFired + firerate < Time.time) {
			print ("test");
			lastFired = Time.time;
			GameObject newBullet = Instantiate<GameObject> (bullet, SpawnPos.transform.position, SpawnPos.transform.rotation);
			Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D> ();
			rb.velocity = GetComponent<Rigidbody2D> ().velocity;
			Vector2 forceOnBullet = SpawnPos.transform.right;
			float theta = Random.Range (-fireField / 2, fireField / 2)*Mathf.Deg2Rad;
			Vector2 rotated = new Vector2 (Mathf.Cos (theta) * forceOnBullet.x - Mathf.Sin (theta) * forceOnBullet.y, Mathf.Sin (theta) * forceOnBullet.x + Mathf.Cos (theta) * forceOnBullet.y);
			rb.AddForce (rotated*bulletVelocity, ForceMode2D.Impulse);
			NetworkServer.Spawn (newBullet);
			Destroy (newBullet, bulletLifetime);
		}
	}
}

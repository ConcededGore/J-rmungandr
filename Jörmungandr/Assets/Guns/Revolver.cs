using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Revolver : WeaponBase {

	// Use this for initialization
	void Start () {
		firerate = 1;
		reloadTime = 3;
		ammoCount = 6;

		bulletLifetime = .33f;
		bulletVelocity = 60;
		fireField = 5;

		autoFire = false;

		lastFired = -firerate;
		currAmmo = ammoCount;
	}

	public override void SingleFire(NetworkInstanceId netId) {
		if (lastFired + firerate < Time.time && (currAmmo > 0 || infiniteAmmo)) {
			lastFired = Time.time;
			GameObject newBullet = Instantiate<GameObject> (bullet, SpawnPos.transform.position, SpawnPos.transform.rotation);
			Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D> ();
			rb.velocity = GetComponent<Rigidbody2D> ().velocity;
			Vector2 forceOnBullet = SpawnPos.transform.right;
			float theta = Random.Range (-fireField / 2, fireField / 2) * Mathf.Deg2Rad;
			Vector2 rotated = new Vector2 (Mathf.Cos (theta) * forceOnBullet.x - Mathf.Sin (theta) * forceOnBullet.y, Mathf.Sin (theta) * forceOnBullet.x + Mathf.Cos (theta) * forceOnBullet.y);
			rb.AddForce (rotated * bulletVelocity, ForceMode2D.Impulse);
			newBullet.GetComponent<CollisionCorrection>().spawnedBy = netId;
			NetworkServer.Spawn (newBullet);
			Destroy (newBullet, bulletLifetime);
			currAmmo--;
			LookAtMouse.Recoil (-45, 6);
		}
	}
}

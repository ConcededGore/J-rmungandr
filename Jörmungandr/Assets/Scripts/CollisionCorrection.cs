using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollisionCorrection : NetworkBehaviour {

	[SyncVar]
	public NetworkInstanceId spawnedBy;
	// Set collider for all clients.
	public override void OnStartClient() {
		GameObject obj = ClientScene.FindLocalObject(spawnedBy);
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
	}
}

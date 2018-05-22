using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletCorrection : NetworkBehaviour {

	private float timer = 2;

	ParticleSystem _particleSystem;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		_particleSystem = GetComponentInChildren<ParticleSystem> ();
		if (isServer) {
			RpcParticles ();
		} else if (isLocalPlayer) {
			CmdParticles ();
		}
	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		transform.right = rb.velocity.normalized;
		Vector3 newScale = transform.localScale;
		newScale.x = rb.velocity.magnitude/80.0f+.1f;
		transform.localScale = newScale;

		if ((_particleSystem != null && timer >= 1)) {
			if (isServer) {
				RpcParticles ();
			} else {
				CmdParticles ();
			}
			timer = 0;
		}
	}

	[Command]
	void CmdParticles() {
		RpcParticles ();
	}

	[ClientRpc]
	void RpcParticles() {
		if (_particleSystem != null) {
			_particleSystem.Emit (1);
		}
	}

}

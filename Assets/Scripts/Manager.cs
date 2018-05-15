using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	List<GameObject> players = new List<GameObject>();

	public int CamZ = -10;

	void Update() {
		GameObject[] playersArray = GameObject.FindGameObjectsWithTag ("Player");
		if (playersArray != null && players.Count != playersArray.GetLength(0)) {
			players.RemoveRange (0, players.Count);
			for (int i = 0; i < playersArray.GetLength (0); i++) {
				players.Add(playersArray [i]);
			}
		}

		foreach (GameObject player in players) {
			if (player.GetComponent<PlayerController> ().IsDead) {
				player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			}
		}
	}

	private Vector3 updateCamPos() {
		Vector3 CamPos = new Vector3 (0, 0, 0);
		CamPos.x = 0;
		CamPos.y = 0;

		for (int i = 0; i < players.Count; i++) {
			if (!float.IsNaN(players [i].transform.position.x) && !float.IsNaN(players [i].transform.position.y)) {
				CamPos.x += players [i].transform.position.x;
				CamPos.y += players [i].transform.position.y;
			}
		}

		if (players.Count > 0) { 
			CamPos /= players.Count;
			CamPos.z = CamZ;
		} else {
			CamPos = new Vector3 (0, 0, CamZ);
		}

		return CamPos;
	}

	public Vector3 GetCamPos {
		get {
			return updateCamPos();
		}
	}

	public List<GameObject> GetPlayers {
		get {
			return players;
		}
	}
}

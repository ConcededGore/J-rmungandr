using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Manager _manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float largestTaxiDistance = 10;
		Camera.main.transform.position = _manager.GetCamPos;
		foreach (GameObject player in _manager.GetPlayers) {
			Vector3 playerDists = player.transform.position;
			largestTaxiDistance = ((Mathf.Abs (playerDists.x - Camera.main.transform.position.x)+1)/ Camera.main.aspect > largestTaxiDistance) ? (Mathf.Abs (playerDists.x - Camera.main.transform.position.x)+1)/Camera.main.aspect : largestTaxiDistance;
			largestTaxiDistance = (Mathf.Abs (playerDists.y - Camera.main.transform.position.y) + 1.5f > largestTaxiDistance) ? Mathf.Abs (playerDists.y - Camera.main.transform.position.y) + 1.5f : largestTaxiDistance;
		}
		Camera.main.orthographicSize = largestTaxiDistance;
	}
}

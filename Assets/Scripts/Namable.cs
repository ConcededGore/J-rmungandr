using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Namable : NetworkBehaviour {

	[SyncVar]
	public string playerName;

	// Use this for initialization
	void Start () {
		name = "Player" + Random.Range (0, 100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

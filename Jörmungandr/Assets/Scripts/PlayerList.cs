using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerList : NetworkBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Network.connections.Length; i++) {
			print (Network.connections [i].ipAddress + " " + Network.connections.Length);
		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sniper : WeaponBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void AutoFire(NetworkInstanceId netId) {
		print ("RATATATAT");
	}
}

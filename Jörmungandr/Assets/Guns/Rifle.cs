﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponBase {

	// Use this for initialization
	void Start () {
		ammoCount = 10;
		currAmmo = ammoCount;
		bulletVelocity = 40;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}

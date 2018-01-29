﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRegenPowerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Players") {
			if (collider.GetComponent<PlayerMovement> ().getNumberOfShots () < 3) {
				collider.gameObject.GetComponent<PlayerMovement> ().updateTurn ();
			}
		}
		Destroy (GameObject);
	}
}
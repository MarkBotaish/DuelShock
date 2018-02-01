using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRegenPowerup : PowerUps {

	GameManagerScript manager;
    TurnObjectParentScript regenCloud;

	// Use this for initialization
	void Start () {
        manager = GameManagerScript.manager;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Players") {
            regenCloud = manager.getRandomDestroyedObject();

        }
	}
}

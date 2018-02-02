using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRegenPowerup : PowerUps {

	GameManagerScript manager;
    TurnObjectParentScript regenCloud;
    PlayerMovement player;

    public int lifeSpan;
    int turns;

    // Use this for initialization
    void Start () {
        manager = GameManagerScript.manager;
        manager.addToUpdateList(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.tag == "Players")
        {
            print("CLOUD REGEN GAINED");
            player = collider.gameObject.GetComponent<PlayerMovement>();

            if (player.getPower() == null)
            {
                player.setPower(this);
                gameObject.SetActive(false);
            }
            else
                print("YOU ALREADY HAVE A POWER UP");

        }
    }

    public override void usePowerUp()
    {
        manager.getRandomDestroyedObject().GetComponent<CloudScript>().resetCloud();
        manager.removeFromUpdateList(this);
        Destroy(gameObject);
    }

    public override void updateTurn()
    {
        turns++;

        if (turns >= lifeSpan)
        {
            manager.removeToUpdateList(this);
            Destroy(gameObject);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRegenPowerup : PowerUps {


    GameManagerScript manager;
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
            print("EXTRA LIGHTNING GAINED");
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
        player.updateTurn();
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

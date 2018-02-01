using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPowerup : PowerUps {

    PlayerMovement player;
    GameManagerScript manager;

    public int lifeSpan;
    int turns;
	// Use this for initialization
	void Start () {
        manager = GameManagerScript.manager;
        manager.addToUpdateList(this);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Players") {
			print ("BARRIER GAINED");
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
        player.turnOnBarrier();
        player.setPower(null);
        manager.removeFromUpdateList(this);
        Destroy(gameObject);
    }

    public override void updateTurn()
    {
        turns++;

        if(turns >= lifeSpan)
        {
            manager.removeToUpdateList(this);
            Destroy(gameObject);
        }
           
    }
}

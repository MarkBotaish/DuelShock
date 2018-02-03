using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRegenPowerup : PowerUps {


    GameManagerScript manager;
    PlayerMovement player;
    public Sprite texture;
    public int lifeSpan;
    int turns;


    // Use this for initialization
    void Start () {
        manager = GameManagerScript.manager;
        manager.addToUpdateList(this);
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
                manager.removeFromUpdateList(this);
            }
            else
                print("YOU ALREADY HAVE A POWER UP");

        }
    }

    public override void usePowerUp()
    {
        if(player.getNumberOfShots() < 3)
        {
            player.updateTurn();
            Destroy(gameObject);
        }
        else
        {
            player.errorBox.GetComponent<ErrorBoxScript>().diplayError("You already have the max number of shots!");
        }
      
    }

    public override void updateTurn()
    {
        turns++;
        if (turns >= lifeSpan && gameObject.activeSelf)
        {
            manager.removeToUpdateList(this);
            Destroy(gameObject);
        }

    }

    public override Sprite getTexture()
    {
        return texture;
    }
}

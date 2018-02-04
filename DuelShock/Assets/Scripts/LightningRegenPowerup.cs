using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRegenPowerup : PowerUps {


    GameManagerScript manager;
    PlayerMovement player;
    public Sprite texture;
    public int lifeSpan;
    int turns;

    GameObject cloud;


    // Use this for initialization
    void Start () {
        manager = GameManagerScript.manager;
        manager.addToUpdateList(this);
    }

	void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.tag == "Players")
        {
            player = collider.gameObject.GetComponent<PlayerMovement>();
            player.errorBox.GetComponent<ErrorBoxScript>().diplayError("EXTRA LIGHTNING GAINED");

            if (player.getPower() == null)
            {
                player.setPower(this);
                gameObject.SetActive(false);
                manager.removeToUpdateList(this);
            }
            else
                player.errorBox.GetComponent<ErrorBoxScript>().diplayError("YOU ALREADY HAVE A POWER UP");

        }
    }

    public override void usePowerUp()
    {
        if(player.getNumberOfShots() < 3)
        {
            player.updateTurn();
            turns = lifeSpan;
            updateTurn();
        }
        else
        {
            player.errorBox.GetComponent<ErrorBoxScript>().diplayError("You already have the max number of shots!");
        }
      
    }

    public override void updateTurn()
    {
        turns++;
        if (turns >= lifeSpan)
        {
            cloud.GetComponent<CloudScript>().setTouched(null);
            manager.removeToUpdateList(this);
            Destroy(gameObject);
        }

    }

    public override Sprite getTexture()
    {
        return texture;
    }
    public override void deleting()
    {
        turns = lifeSpan;
        updateTurn();
    }

    public override void init(GameObject obj) {
        cloud = obj;
    }
}

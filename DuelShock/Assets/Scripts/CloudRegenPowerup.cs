using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRegenPowerup : PowerUps {

	GameManagerScript manager;
    TurnObjectParentScript regenCloud;
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
            print("CLOUD REGEN GAINED");
            player = collider.gameObject.GetComponent<PlayerMovement>();
            manager.removeFromUpdateList(this);
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
        int numberOfClouds = manager.getNumberOfCloudsDestroyed();
        if (numberOfClouds > 0)
        {
            print(numberOfClouds);
            if(numberOfClouds >= 2)
                manager.getRandomDestroyedObject().GetComponent<CloudScript>().resetCloud();

            manager.getRandomDestroyedObject().GetComponent<CloudScript>().resetCloud();
            Destroy(gameObject);
        }
        else
        {
            player.errorBox.GetComponent<ErrorBoxScript>().diplayError("No clouds have been destroyed!");
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

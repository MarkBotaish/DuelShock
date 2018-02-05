using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRegenPowerup : PowerUps {

	GameManagerScript manager;
    PlayerMovement player;
    GameObject cloud;
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
            player = collider.gameObject.GetComponent<PlayerMovement>();

            if (player.getPower() == null)
            {
                cloud.GetComponent<CloudScript>().setTouched(player);
                player.errorBox.GetComponent<ErrorBoxScript>().diplayError("CLOUD REGEN GAINED");
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
        int numberOfClouds = manager.getNumberOfCloudsDestroyed();
        if (numberOfClouds > 0)
        {
            cloud.GetComponent<CloudScript>().setTouched(player);
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

    public override void deleting()
    {
        Destroy(gameObject);
    }

    public override void init(GameObject obj)
    {
        cloud = obj;
    }
}



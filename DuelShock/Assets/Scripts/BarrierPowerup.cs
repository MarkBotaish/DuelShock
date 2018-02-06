using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPowerup : PowerUps {

    PlayerMovement player;
    GameManagerScript manager;
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
		if (collider.tag == "Players") {
            player = collider.gameObject.GetComponent<PlayerMovement>();
            
            if (player.getPower() == null)
            {
                cloud.GetComponent<CloudScript>().setTouched(player);
                player.errorBox.GetComponent<ErrorBoxScript>().diplayError("BARRIER GAINED");
                player.setPower(this);
                gameObject.SetActive(false);
                manager.removeFast(this);
            }
            else
                player.errorBox.GetComponent<ErrorBoxScript>().diplayError("YOU ALREADY HAVE A POWER UP");

        }
	}
    public override void usePowerUp()
    {
      
        player.turnOnBarrier();
        player.setPower(null);
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
    public override Sprite getTexture()
    {
        return texture;
    }

    public override void deleting()
    {
        if (gameObject.activeSelf)
            Destroy(gameObject);
    }

    public override void init(GameObject obj)
    {
        cloud = obj;
    }
}

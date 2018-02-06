using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : TurnObjectParentScript
{
    public float reducedSpeed;
    GameManagerScript manager;
    bool canHighlight = false;
    bool hasBeenPicked = false;
    bool hasNoted = false;

    GameObject partical;
    GameObject player = null;
    TurnObjectParentScript objectOnCloud;

    // Use this for initialization
    int turnDestroy = 0;
	void Start () {
        manager = GameManagerScript.manager;
        partical = transform.GetChild(0).gameObject;
        //objectOnCloud = null; 
	}

    public override void updateTurn()
    {
        
        turnDestroy++;
        if (turnDestroy >= 1)
        {
            manager.removeToUpdateList(this);
        }
        
        if (hasNoted)
        {
            gameObject.GetComponent<SpriteRenderer>().color += new Color(1.0f, 1.0f, 1.0f, 0.0f);
            hasNoted = false;
        }
    }

    private void OnMouseDown()
    {
        if (canHighlight && !hasBeenPicked)
        {
            PlayerMovement player = manager.getCurrentPlayer().GetComponent<PlayerMovement>();
            hasNoted = true;
            if (player.getNumberOfShots() > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color -= new Color(1.0f, 1.0f, 1.0f, 0.0f);
                manager.addToUpdateList(this);
                player.shoot();
            }
            else
            {
                manager.displayError("Can't Shoot");
            }
            hasBeenPicked = true;
        }
    }
    public void strikeLightning()
    {
        if(objectOnCloud == null)
        {
            partical.SetActive(true);
            gameObject.GetComponent<Animator>().SetBool("hasStuck", false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            manager.addToDestroyedObject(this);
            manager.removeFast(this);
        }
        else
        {
            if (objectOnCloud.tag == "Player")
                player.GetComponent<PlayerMovement>().dealDamage();
            else
            {
                objectOnCloud.deleting();
               
            }
            resetCloud();
        }
       

		if(player != null)
        {
            player.GetComponent<PlayerMovement>().dealDamage();

        }
    }

 
    private void OnMouseEnter()
    {
        if (manager.cloudPlayerCheck(gameObject))
        {
            gameObject.GetComponent<SpriteRenderer>().color -= new Color(0.0f, 0.25f, 0.25f, 0.0f);
            canHighlight = true;
        }
        else
        {
            canHighlight = false;
        }
           
    }

    private void OnMouseExit()
    {
        if(canHighlight)
            gameObject.GetComponent<SpriteRenderer>().color += new Color(0.0f, 0.25f, 0.25f, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Cloud" && objectOnCloud == null)
        {
            objectOnCloud = collision.gameObject.GetComponent<TurnObjectParentScript>();
        }
        if (collision.tag == "Players")
        {
            collision.gameObject.GetComponent<PlayerMovement>().touched(gameObject);
            player = collision.gameObject;

            if(!gameObject.GetComponent<SpriteRenderer>().enabled)
                collision.gameObject.GetComponent<PlayerMovement>().setSpeed(reducedSpeed);
        }
    }
            
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Cloud" && objectOnCloud != null && objectOnCloud.tag != "Power")
        {
            objectOnCloud = null;
        }

        if (collision.tag == "Players")
        {
            collision.gameObject.GetComponent<PlayerMovement>().released(gameObject);
           
            player = null;

            if (gameObject.GetComponent<SpriteRenderer>().enabled)
                collision.gameObject.GetComponent<PlayerMovement>().restSpeed();
        }
    }

    public override void deleting()
    {
        gameObject.GetComponent<Animator>().SetBool("hasStuck", true);
    }

    public void resetCloud()
    {
        partical.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Animator>().SetBool("hasStuck", false);
        hasBeenPicked = false;
        manager.removeFast(this);
        manager.removeToDestroyedObject(this);
        hasBeenPicked = false;
        turnDestroy = 0;
    }

    public void setTouched(TurnObjectParentScript obj)
    {
        objectOnCloud = obj;
    }

    public TurnObjectParentScript getTouched()
    {
        return objectOnCloud;
    }

}

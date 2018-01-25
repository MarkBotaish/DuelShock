using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : TurnObjectParentScript
{

    GameManagerScript manager;
    bool canHighlight = false;
    // Use this for initialization
    int turnDestroy = 0;
	void Start () {
        manager = GameManagerScript.manager;
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void updateTurn()
    {
        turnDestroy++;
        if (turnDestroy >= 2)
        {
            manager.removeToUpdateList(this);
            gameObject.SetActive(false);
        }
           
    }

    private void OnMouseDown()
    {
        if (canHighlight)
        {
            manager.addToUpdateList(this);

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : TurnObjectParentScript {

    float xMove, yMove;
    public float speed;
    public int maxShots;
    public Text Shots;
    public Text health;
    public int lives;

    int numberOfShots = 0;
    float fullSpeed;
    float lowerXBound, upperXBound;
    float lowerYBound, upperYBound;

    bool canMove = false;

	bool hasBarrier = false;

    List<GameObject> touchedObjects = new List<GameObject>();

    PowerUps power;

	// Use this for initialization
	void Start () {
        power = null;
        fullSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        if(canMove)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xMove * speed, yMove * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && power!= null)
            power.usePowerUp();
       
	}
    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void restSpeed()
    {
        speed = fullSpeed;
    }

    public void setMove(bool tof)
    {
        canMove = tof;
    }
    public void stopMovement()
    {
        canMove = false;
        GameObject min = touchedObjects[0];
        float minDist = float.MaxValue;

        foreach(GameObject cloudTouched in touchedObjects)
        {
            if (Vector2.Distance(transform.position, cloudTouched.transform.position) < minDist)
            {
                minDist = Vector2.Distance(transform.position, cloudTouched.transform.position);
                min = cloudTouched;
            }
        }

        transform.position = min.transform.position;

        if (!min.GetComponent<SpriteRenderer>().enabled)
        {
            dealDamage();
        }
            

    }

    public void touched(GameObject collision)
    {
        touchedObjects.Add(collision.gameObject);
    }

    public void released(GameObject collision)
    {
        touchedObjects.Remove(collision.gameObject);
    }

    public override void updateTurn()
    {
        numberOfShots++;
        if (numberOfShots > 3)
            numberOfShots = 3;

        Shots.text = "Shots: " + numberOfShots + "/" + maxShots;
    }

    public int getNumberOfShots()
    {
        return numberOfShots;
    }

    public void shoot()
    {
        numberOfShots--;
        Shots.text = "Shots: " + numberOfShots + "/" + maxShots;
    }

    public void dealDamage()
    {
		if (!hasBarrier) {
			print ("DAMAGE DEALT");
			lives--;
			if (lives <= 0)
				print ("YOU DEAD AS HELL");
		} else {
			print ("DAMAGE ABSORBED");
            hasBarrier = false;
		}

        health.text = lives.ToString();
    }

    public PowerUps getPower()
    {
        return power;
    }

    public void setPower(PowerUps powerUp)
    {
        power = powerUp;
    }

    public void turnOnBarrier()
    {
        hasBarrier = true;
        print("BARRIER ON");
    }
}

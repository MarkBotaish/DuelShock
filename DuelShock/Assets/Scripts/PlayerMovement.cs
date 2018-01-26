using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float xMove, yMove;
    public float speed;
    public int maxShots;

    int numberOfShots;
    float lowerXBound, upperXBound;
    float lowerYBound, upperYBound;

    bool canMove = false;

    List<GameObject> touchedObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        if(canMove)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xMove * speed, yMove * speed);
        }
       
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

    }

    public void touched(GameObject collision)
    {
        touchedObjects.Add(collision.gameObject);
    }

    public void released(GameObject collision)
    {
        touchedObjects.Remove(collision.gameObject);
    }
}

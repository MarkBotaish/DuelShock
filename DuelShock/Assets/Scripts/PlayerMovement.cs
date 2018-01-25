using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float xMove, yMove;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xMove * speed, yMove* speed);
	}
}

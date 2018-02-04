using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : TurnObjectParentScript {

    GameManagerScript manager;
    bool isMoving = false;
    bool hasMoved = false;
    int turn = -1;

    public float speed;

    //So Unity doesnt have to calculate this varaibles 3+ times in one run of the update function
    Vector3 cameraTemp;
    Vector3 finalPosTemp;

	// Use this for initialization
	void Start () {
        //Init the varaibles to zero s
        cameraTemp = Vector3.zero;
        finalPosTemp = Vector3.zero;
    }

    public void init()
    {
        manager = GameManagerScript.manager;
    }

    void Update()
    {

        if (isMoving)
        {
            gameObject.transform.position += new Vector3(speed * turn, 0.0f, 0.0f);
            if(turn > 0)
            {
                cameraTemp = transform.position;
                finalPosTemp = manager.getPlayerTwoCameraPosition();
                if (cameraTemp.x >= finalPosTemp.x)
                {
                    transform.position = new Vector3(finalPosTemp.x, finalPosTemp.y, cameraTemp.z);
                    isMoving = false;
                }
            }
            else
            {
                cameraTemp = transform.position;
                finalPosTemp = manager.getPlayerOneCameraPosition();
                if (cameraTemp.x <= finalPosTemp.x)
                {
                    transform.position = new Vector3(finalPosTemp.x, finalPosTemp.y, cameraTemp.z);
                    isMoving = false;
                }
            }
        }
    }

    public override void updateTurn()
    {
       
        if (!hasMoved)
        {
            isMoving = true;
        }
        else
        {
            //If the camera has move(an odd number of times) the turn is not correct. This fixes the turn 
            turn *= -1;
        }
        turn *= -1;
        hasMoved = false;

    }

    public void moveCamera()
    {
  
        isMoving = true;
        hasMoved = !hasMoved;
        turn *= -1;
    }
}

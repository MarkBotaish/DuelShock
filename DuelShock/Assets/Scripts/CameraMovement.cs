using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : TurnObjectParentScript {

    GameManagerScript manager;
    bool isMoving = false;
    int turn = -1;

    public float speed;
    Vector3 cameraTemp;
    Vector3 finalPosTemp;

	// Use this for initialization
	void Start () {
        cameraTemp = Vector3.zero;
        finalPosTemp = Vector3.zero;
    }

    public void init()
    {
        manager = GameManagerScript.manager;
        print(manager);
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
        turn *= -1;
        print(turn);
        isMoving = true;
    }
}

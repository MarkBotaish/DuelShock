using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript manager;

    public float boardOneXPos;
    public float boardOneYPos;
    public float boardTwoXPos;
    public float boardTwoYPos;

    public int numOfCols;
    public int numOfRows;
    public int powerUpsSpawnRound;

    public float timeToMove;
    public float speedMultiplier;
    public int numberOfRoundsForMultiplier;

    public GameObject cloudPrefab;
    public TurnObjectParentScript playerCamera;
    public TurnObjectParentScript playerOne;
    public TurnObjectParentScript playerTwo;
    public GameObject wall;
    public Button start;

    public Text turnText;

    GameObject[,] firstBoard;
    GameObject[,] secondBoard;

    List<TurnObjectParentScript> destroyedFirstBoard = new List<TurnObjectParentScript>();
    List<TurnObjectParentScript> destroyedSecondBoard = new List<TurnObjectParentScript>();

    bool hasMoved = false;


    public List<GameObject> powerUps;

    //This is the list of all objects that need to be updated on turns. Powerups and clouds go in this list
    List<TurnObjectParentScript> updateTurnList = new List<TurnObjectParentScript>();

    //Cannot edit list while looping through. This is a temp lis to hold the removed objects
    List<TurnObjectParentScript> removeList = new List<TurnObjectParentScript>();

    int playersTurn = 0;

    Vector2 playerOneCameraPosition;
    Vector2 playerTwoCameraPosition;

    // Use this for initialization
    void Start()
    {
        manager = this;
        firstBoard = new GameObject[numOfRows, numOfCols];
        secondBoard = new GameObject[numOfRows, numOfCols];

        //Sets the camera pos to the center of player one's board
        playerOneCameraPosition = new Vector2(boardOneXPos + ((numOfCols - 1.0f) / 2.0f), boardOneYPos - ((numOfRows - 1.0f) / 2.0f));

        //Sets the camera pos to the center of player two's board
        playerTwoCameraPosition = new Vector2(boardTwoXPos + ((numOfCols - 1.0f) / 2.0f), boardTwoYPos - ((numOfRows - 1.0f) / 2.0f));

        addToUpdateList(playerCamera);
        playerCamera.GetComponent<CameraMovement>().init();

        turnText.text = "Player " + (playersTurn + 1) + "'s turn!";

        spawnWall();
        buildBoard();

        playerOne.transform.position = playerOneCameraPosition;
        playerTwo.transform.position = playerTwoCameraPosition;

        updateTurnList.Add(playerOne);
        updateTurnList.Add(playerTwo);

        playerOne.updateTurn();


    }

    public Vector2 getPlayerOneCameraPosition()
    {
        return playerOneCameraPosition;
    }
    public Vector2 getPlayerTwoCameraPosition()
    {
        return playerTwoCameraPosition;
    }

    public void addToUpdateList(TurnObjectParentScript objectToAdd)
    {
        updateTurnList.Add(objectToAdd);
    }

    public void removeToUpdateList(TurnObjectParentScript objectToAdd)
    {
        removeList.Add(objectToAdd);
        if(objectToAdd.tag == "Cloud")
        {
            destroyedFirstBoard.Add(objectToAdd);
        }
    }
    public void removeFromUpdateList(TurnObjectParentScript objectToRemove)
    {
        updateTurnList.Remove(objectToRemove);
    }
    public bool cloudPlayerCheck(GameObject cloud)
    {
        //if player 1
        if (playersTurn % 2 == 0)
        {
            //If the cloud is in the opposite board return true; otherwise, return false;
            foreach (GameObject cloudObject in secondBoard)
            {
                if (cloudObject == cloud)
                    return true;
            }
            return false;
        }
        //If player 2
        if (playersTurn % 2 == 1)
        {
            //If the cloud is in the opposite board return true; otherwise, return false;
            foreach (GameObject cloudObject in firstBoard)
            {
                if (cloudObject == cloud)
                    return true;
            }
            return false;
        }
        Debug.Log("Something went wrong...");
        return false;
    }

    public TurnObjectParentScript getRandomDestroyedObject()
    {
        if (playersTurn % 2 == 0)
            return destroyedFirstBoard[Random.Range(0, destroyedFirstBoard.Count - 1)];
        return destroyedSecondBoard[Random.Range(0, destroyedSecondBoard.Count - 1)];
    }

    //Creates the map
    void buildBoard()
    {
       
        for (int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                //Builds player 1's map and adds it to a 2D array 
                GameObject first = (GameObject)Instantiate(cloudPrefab, new Vector2(boardOneXPos, boardOneYPos), Quaternion.identity).gameObject;
                firstBoard[i,j] = first;

                //Builds player 2's map and adds it to a 2D array 
                GameObject second = (GameObject)Instantiate(cloudPrefab, new Vector2(boardTwoXPos, boardTwoYPos), Quaternion.identity).gameObject;
                secondBoard[i,j] = second;

                boardOneXPos++; 
                boardTwoXPos++;
            }
            boardOneXPos -= numOfCols;
            boardTwoXPos -= numOfCols;
            boardOneYPos--;
            boardTwoYPos--;
        }
    }
    //Keeps the playersTurn between 0 and 1
    public void updateTurn()
    {
        if (hasMoved)
        {
            start.gameObject.SetActive(true);
            playersTurn++;
            spawnPowerUps();

            turnText.text = "Player " + (playersTurn % 2 + 1) + "'s turn!";
            foreach (TurnObjectParentScript update in updateTurnList)
            {
                if (update.tag == "Players")
                {
                    if (playersTurn % 2 == 0 && update == playerOne)
                        playerOne.updateTurn();
                    else if (playersTurn % 2 == 1 && update == playerTwo)
                        playerTwo.updateTurn();


                }
                else
                {
                    update.updateTurn();
                }

            }

            foreach (TurnObjectParentScript removeObject in removeList)
            {
                updateTurnList.Remove(removeObject);
            }

            playerOne.GetComponent<PlayerMovement>().enabled = !playerOne.GetComponent<PlayerMovement>().enabled;
            playerTwo.GetComponent<PlayerMovement>().enabled = !playerTwo.GetComponent<PlayerMovement>().enabled;

            checkSpeed();
            hasMoved = false;
        }
        else
        {
            print("NEED TO MOVE FIRST");
        }
        
    }

    void spawnPowerUps()
    {
        if (playersTurn % powerUpsSpawnRound == 0)
            Instantiate(powerUps[Random.Range(0, powerUps.Count)], firstBoard[Random.Range(0, numOfRows), Random.Range(0, numOfCols)].transform.position, Quaternion.identity);
        if (playersTurn % (powerUpsSpawnRound+1) == 0)
            Instantiate(powerUps[Random.Range(0, powerUps.Count)], secondBoard[Random.Range(0, numOfRows), Random.Range(0, numOfCols)].transform.position, Quaternion.identity);
    }

    void checkSpeed()
    {
        if(playersTurn % numberOfRoundsForMultiplier == 0)
        {
            //Check For better solution 
            foreach (GameObject clouds in firstBoard)
                clouds.GetComponent<Animator>().speed *= speedMultiplier;
            foreach (GameObject clouds in secondBoard)
                clouds.GetComponent<Animator>().speed *= speedMultiplier;
        }
    }

    public void startTimeToMove()
    {
        hasMoved = true;
        foreach (TurnObjectParentScript removeObject in removeList)
        {
            removeObject.deleting();
        }

        removeList.Clear();
        start.gameObject.SetActive(false);
        if (playersTurn % 2 == 0)
            playerOne.GetComponent<PlayerMovement>().setMove(true);
        else
            playerTwo.GetComponent<PlayerMovement>().setMove(true);
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(timeToMove);
        if (playersTurn % 2 == 0)
        {
            playerOne.GetComponent<PlayerMovement>().stopMovement();
            playerOne.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
           
        else{
            playerTwo.GetComponent<PlayerMovement>().stopMovement();
            playerTwo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
           

    }

    void spawnWall()
    {
        //Player ones walls
        GameObject sideWalls = Instantiate(wall, new Vector2(playerOneCameraPosition.x, playerOneCameraPosition.y + ((numOfRows - 1.0f) / 2.0f)+1.0f), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(numOfCols, 1.0f, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerOneCameraPosition.x, playerOneCameraPosition.y - ((numOfRows - 1.0f) / 2.0f) - 1.0f), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(numOfCols, 1.0f, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerOneCameraPosition.x - ((numOfCols - 1.0f) / 2.0f) -1.0f, playerOneCameraPosition.y), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(1.0f, numOfRows, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerOneCameraPosition.x + ((numOfCols - 1.0f) / 2.0f) + 1.0f, playerOneCameraPosition.y), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(1.0f, numOfRows, 1.0f);

        //Player two walls
        sideWalls = Instantiate(wall, new Vector2(playerTwoCameraPosition.x, playerTwoCameraPosition.y + ((numOfRows - 1.0f) / 2.0f) + 1.0f), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(numOfCols, 1.0f, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerTwoCameraPosition.x, playerTwoCameraPosition.y - ((numOfRows - 1.0f) / 2.0f) - 1.0f), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(numOfCols, 1.0f, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerTwoCameraPosition.x - ((numOfCols - 1.0f) / 2.0f) - 1.0f, playerTwoCameraPosition.y), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(1.0f, numOfRows, 1.0f);
        sideWalls = Instantiate(wall, new Vector2(playerTwoCameraPosition.x + ((numOfCols - 1.0f) / 2.0f) + 1.0f, playerTwoCameraPosition.y), Quaternion.identity).gameObject;
        sideWalls.transform.localScale = new Vector3(1.0f, numOfRows, 1.0f);
    }

    public TurnObjectParentScript getCurrentPlayer()
    {
        if (playersTurn % 2.0 == 0)
            return playerOne;
        
        return playerTwo;
    }
}

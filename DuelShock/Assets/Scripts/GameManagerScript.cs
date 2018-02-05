using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript manager;

    float boardOneXPos = -9.0f;
    float boardOneYPos = 1.5f;
    float boardTwoXPos = 4.0f;
    float boardTwoYPos = 1.5f;

    int numOfCols = 6;
    int numOfRows = 4;
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
    public GameObject camera;
    public GameObject error;

    public Text turnText;

    GameObject[,] firstBoard;
    GameObject[,] secondBoard;

    List<TurnObjectParentScript> destroyedFirstBoard = new List<TurnObjectParentScript>();
    List<TurnObjectParentScript> destroyedSecondBoard = new List<TurnObjectParentScript>();

    bool hasMoved = false;
    bool isMoving = false;

    Coroutine cTimer;

    public List<TurnObjectParentScript> powerUps;

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

        playerOne.transform.position = firstBoard[(numOfRows / 2), (numOfCols / 2)].transform.position;
        playerTwo.transform.position = secondBoard[(numOfRows / 2), (numOfCols / 2)].transform.position;

        firstBoard[(numOfRows / 2), (numOfCols / 2)].GetComponent<CloudScript>().setTouched(playerOne);
        secondBoard[(numOfRows / 2), (numOfCols / 2)].GetComponent<CloudScript>().setTouched(playerTwo);

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
    }

    public void removeFast(TurnObjectParentScript objectToAdd)
    {
        updateTurnList.Remove(objectToAdd);
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
        TurnObjectParentScript temp;
        if (playersTurn % 2 == 0)
            temp = destroyedFirstBoard[Random.Range(0, destroyedFirstBoard.Count)];
        else
            temp = destroyedSecondBoard[Random.Range(0, destroyedSecondBoard.Count)];

        removeFromDestroyedClouds(temp);
        return temp;
    }

    void removeFromDestroyedClouds(TurnObjectParentScript obj)
    {
        if (destroyedFirstBoard.Contains(obj))
            destroyedFirstBoard.Remove(obj);
        else destroyedSecondBoard.Remove(obj);
    }

    public int getNumberOfCloudsDestroyed()
    {
        if (playersTurn % 2 == 0)
            return destroyedFirstBoard.Count;
        return destroyedSecondBoard.Count;
    }

    public void addToDestroyedObject(TurnObjectParentScript obj)
    {
        if (playersTurn % 2 == 0)
            destroyedFirstBoard.Add(obj);
       else
            destroyedSecondBoard.Add(obj);
    }

    public void removeToDestroyedObject(TurnObjectParentScript obj)
    {
        if (playersTurn % 2 == 0)
            destroyedFirstBoard.Remove(obj);
        else
            destroyedSecondBoard.Remove(obj);
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
    public void updateTurn()
    {

        if (hasMoved)
        {
            //Stops the player if they end their turn before the timer ends
            if (isMoving)
            {
                StopCoroutine(cTimer);
                if (playersTurn % 2 == 0)
                {
                    playerOne.GetComponent<PlayerMovement>().stopMovement();
                    playerOne.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else
                {
                    if(playersTurn >= 1)
                    {
                        playerTwo.GetComponent<PlayerMovement>().stopMovement();
                        playerTwo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }
                isMoving = false;
            }

            start.gameObject.SetActive(true);
            playersTurn++;
            spawnPowerUps();

            turnText.text = "Player " + (playersTurn % 2 + 1) + "'s turn!";
            for (int i = 0; i < updateTurnList.Count; i++)
            {
                if (updateTurnList[i] == null)
                {
                    updateTurnList.Remove(updateTurnList[i]);
                    i--;
                    continue;
                }
                if (updateTurnList[i].tag == "Players")
                {
                    if (playersTurn % 2 == 0 && updateTurnList[i] == playerOne)
                        playerOne.updateTurn();
                    else if (playersTurn % 2 == 1 && updateTurnList[i] == playerTwo)
                        playerTwo.updateTurn();
                }
                else
                {
                    updateTurnList[i].updateTurn();
                }
            }
            playerOne.GetComponent<PlayerMovement>().enabled = !playerOne.GetComponent<PlayerMovement>().enabled;
            playerTwo.GetComponent<PlayerMovement>().enabled = !playerTwo.GetComponent<PlayerMovement>().enabled;

            checkSpeed();
            hasMoved = false;
        }
        else
        {
            error.GetComponent<ErrorBoxScript>().diplayError("NEED TO MOVE FIRST");
        }

    }

    void spawnPowerUps()
    {
        int row = Random.Range(0, numOfRows);
        int col = Random.Range(0, numOfRows);
        int powerUpNumber = Random.Range(0, powerUps.Count);
        if (playersTurn % powerUpsSpawnRound == 0)
        {
            while (firstBoard[row, col].GetComponent<CloudScript>().getTouched() != null)
            {
                row = Random.Range(0, numOfRows);
                col = Random.Range(0, numOfRows);
            }

            PowerUps P = (PowerUps)Instantiate(powerUps[powerUpNumber], firstBoard[row, col].transform.position, Quaternion.identity);
            firstBoard[row, col].GetComponent<CloudScript>().setTouched(P);
            P.init(firstBoard[row, col]);
        }
            
        if (playersTurn % (powerUpsSpawnRound+1) == 0)
        {

            while (secondBoard[row, col].GetComponent<CloudScript>().getTouched() != null)
            {
                row = Random.Range(0, numOfRows);
                col = Random.Range(0, numOfRows);
            }

            PowerUps P = (PowerUps)Instantiate(powerUps[powerUpNumber], secondBoard[row, col].transform.position, Quaternion.identity);
            secondBoard[row, col].GetComponent<CloudScript>().setTouched(P);
            P.init(secondBoard[row, col]);

        }
           


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
        for(int i = 0; i < removeList.Count; i++)
        {
            if (removeList[i] != null)
                removeList[i].deleting();
        }
       if(removeList.Count > 0)
            removeList.Clear();


        start.gameObject.SetActive(false);
        if (playersTurn % 2 == 0)
            playerOne.GetComponent<PlayerMovement>().setMove(true);
        else
            playerTwo.GetComponent<PlayerMovement>().setMove(true);
        cTimer = StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        isMoving = true;
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
        isMoving = false;
        
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<CameraMovement>().moveCamera();
        error.GetComponent<ErrorBoxScript>().diplayError("Select a tile to attack!");

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

    public void displayError(string msg)
    {
        error.GetComponent<ErrorBoxScript>().diplayError(msg);
    }

    public TurnObjectParentScript getCurrentPlayer()
    {
        if (playersTurn % 2.0 == 0)
            return playerOne;
        
        return playerTwo;
    }
}

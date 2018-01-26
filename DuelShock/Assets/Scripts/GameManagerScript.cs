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

    public float timeToMove;
    public float timeForLight;

    public GameObject cloudPrefab;
    public TurnObjectParentScript playerCamera;
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject wall;
    public Button start;

    public Text turnText;

    GameObject[,] firstBoard;
    GameObject[,] secondBoard;

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
    public bool cloudPlayerCheck(GameObject cloud)
    {
        //if player 1
        if (playersTurn == 0)
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
        if (playersTurn == 1)
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
        start.gameObject.SetActive(true);
        playersTurn = ++playersTurn % 2;
        turnText.text = "Player " + (playersTurn + 1) + "'s turn!";
        foreach(TurnObjectParentScript update in updateTurnList)
        {
            update.updateTurn();
        }
        
        foreach(TurnObjectParentScript removeObject in removeList)
        {
            updateTurnList.Remove(removeObject);
        }
        playerOne.GetComponent<PlayerMovement>().enabled = !playerOne.GetComponent<PlayerMovement>().enabled;
        playerTwo.GetComponent<PlayerMovement>().enabled = !playerTwo.GetComponent<PlayerMovement>().enabled;


    }

    public void startTimeToMove()
    {
        start.gameObject.SetActive(false);
        if (playersTurn == 0)
            playerOne.GetComponent<PlayerMovement>().setMove(true);
        else
            playerTwo.GetComponent<PlayerMovement>().setMove(true);
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(timeToMove);
        if (playersTurn == 0)
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
}

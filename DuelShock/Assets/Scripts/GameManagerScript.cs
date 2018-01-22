using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public float boardOneXPos;
    public float boardOneYPos;
    public float boardTwoXPos;
    public float boardTwoYPos;

    public int numberOfRows;
    public int numberOfCols;

    public GameObject cloudPrefab;

    // Use this for initialization
    void Start()
    {
        buildBoard();
    }

    void buildBoard()
    {
        for (int i = 0; i < numberOfCols; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                Instantiate(cloudPrefab, new Vector2(boardOneXPos, boardOneYPos), Quaternion.identity);
                Instantiate(cloudPrefab, new Vector2(boardTwoXPos, boardTwoYPos), Quaternion.identity);
                boardOneXPos++;
                boardTwoXPos++;
            }
            boardOneXPos -= numberOfRows;
            boardTwoXPos -= numberOfRows;
            boardOneYPos--;
            boardTwoYPos--;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript manager;

    public float boardOneXPos;
    public float boardOneYPos;
    public float boardTwoXPos;
    public float boardTwoYPos;

    public int numberOfRows;
    public int numberOfCols;

    public GameObject cloudPrefab;

    GameObject[,] firstBoard;
    GameObject[,] secondBoard;

    //This is the list of all objects that need to be updated on turns. Powerups and clouds go in this list
    List<TurnObjectParentScript> updateTurnList = new List<TurnObjectParentScript>();

    //Cannot edit list while looping through. This is a temp lis to hold the removed objects
    List<TurnObjectParentScript> removeList = new List<TurnObjectParentScript>();

    int playersTurn = 0;

    // Use this for initialization
    void Start()
    {
        manager = this;
        firstBoard = new GameObject[numberOfRows, numberOfCols];
        secondBoard = new GameObject[numberOfRows, numberOfCols];
       
        buildBoard();
       
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
       
        for (int i = 0; i < numberOfCols; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
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
            boardOneXPos -= numberOfRows;
            boardTwoXPos -= numberOfRows;
            boardOneYPos--;
            boardTwoYPos--;
        }
    }
    //Keeps the playersTurn between 0 and 1
    public void updateTurn()
    {
        playersTurn = ++playersTurn % 2;
        foreach(TurnObjectParentScript update in updateTurnList)
        {
            update.updateTurn();
        }
        
        foreach(TurnObjectParentScript removeObject in removeList)
        {
            updateTurnList.Remove(removeObject);
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRegenPowerup : MonoBehaviour {

	int count = 0;

	GameObject manager;

	List<int> destroyedCloudRow = new List<int> ();
	List<int> destroyedCloudCol = new List<int> ();

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Game Manager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Players") {
			getDestroyedClouds (collider.name);
			int x = Random.Range (0, count - 1);
			int y = Random.Range (0, count - 1);

			if (collider.name == "Player One") {
				manager.GetComponent<GameManagerScript> ().firstBoard [x, y].GetComponent<SpriteRenderer> ().enabled = true;
			}
			else
				manager.GetComponent<GameManagerScript> ().secondBoard [x, y].GetComponent<SpriteRenderer> ().enabled = true;

			x = Random.Range (0, count - 1);
			y = Random.Range (0, count - 1);

			if (collider.name == "Player One") {
				manager.GetComponent<GameManagerScript> ().firstBoard [x, y].GetComponent<SpriteRenderer> ().enabled = true;
			}
			else
				manager.GetComponent<GameManagerScript> ().secondBoard [x, y].GetComponent<SpriteRenderer> ().enabled = true;

			Destroy (gameObject);
		}
	}

	void getDestroyedClouds(string playerName)
	{
		GameObject[,] firstBoardTemp = manager.GetComponent<GameManagerScript> ().firstBoard;
		GameObject[,] secondBoardTemp = manager.GetComponent<GameManagerScript> ().secondBoard;

		for (int i = 0; i < manager.GetComponent<GameManagerScript> ().numOfRows; i++) {
			for (int j = 0; j < manager.GetComponent<GameManagerScript> ().numOfCols; j++) {
				if (playerName == "Player One") {
					if (!firstBoardTemp [i, j].GetComponent<SpriteRenderer> ().enabled) {
						destroyedCloudRow.Add (i);
						destroyedCloudCol.Add (j);
						count++;
					}
				} else {
					if (!secondBoardTemp [i, j].GetComponent<SpriteRenderer> ().enabled) {
						destroyedCloudRow.Add (i);
						destroyedCloudCol.Add (j);
						count++;
					}
				}
			}
		}
	}
}

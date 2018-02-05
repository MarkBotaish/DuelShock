using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

	public void StartGame()
	{
		SceneManager.LoadScene ("main");
	}

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene("Help Menu");
    }

    public void QuitGame()
	{
		Application.Quit ();
	}
}

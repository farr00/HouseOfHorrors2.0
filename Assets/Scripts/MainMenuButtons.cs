using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
	public Transform instructionCanvas;

	public void StartGameB ()
	{
		SceneManager.LoadScene ("Building A F3");
	}

	public void ExitGameB ()
	{
		Debug.Log ("Quit");
		Application.Quit ();
	}

	public void InstructionsB ()
	{
		if (instructionCanvas.gameObject.activeInHierarchy == false) {
			instructionCanvas.gameObject.SetActive (true);
		} else {
			instructionCanvas.gameObject.SetActive (false);
		}
	}
}

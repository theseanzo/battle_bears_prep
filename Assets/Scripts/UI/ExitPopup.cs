using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitPopup : MonoBehaviour
{
	
	public void OnYesButton()
	{
		GameScreen.isPaused = false;
		SceneManager.LoadScene("MenuScene");
	}

	public void OnNoButton()
	{
		GameScreen.isPaused = false;
		gameObject.SetActive(false);
	}
}

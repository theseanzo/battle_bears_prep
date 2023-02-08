using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
	void Start()
	{
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 1;

	}

	public void OnStartGameButton()
	{
		SceneManager.LoadScene("GameScene");
	}
}

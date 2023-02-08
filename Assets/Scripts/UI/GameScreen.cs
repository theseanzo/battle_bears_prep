using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
	public ExitPopup exitPopup;

	public Text textPrefab;

	public static bool isPaused;

	private Text[] textFields;
	private int nTeams;

	void Start()
	{
		nTeams = GameManager.instance.teams.Length;
		textFields = new Text[nTeams];
		for (int i = 0; i < nTeams; i++)
		{
			Text textField = Instantiate(textPrefab);
			textField.transform.SetParent(textPrefab.transform.parent, false);
			textField.color = GameManager.instance.teams [i];
			textFields [i] = textField;
		}
		Destroy(textPrefab.gameObject);
	}

	void Update()
	{
		for (int i = 0; i < nTeams; i++)
		{
			textFields [i].text = ScoreManager.instance.scores [i].ToString();
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			isPaused = !isPaused;
			exitPopup.gameObject.SetActive(isPaused);
		}
		if (isPaused)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Time.timeScale = 0;
		} else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Time.timeScale = 1;
		}
	}


}

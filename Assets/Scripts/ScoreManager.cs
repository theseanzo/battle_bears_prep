using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{

	public static ScoreManager instance;

	public int[] scores;

	private void Awake()
	{
		instance = this;
		scores = new int[GameManager.instance.teams.Length];
	}


}

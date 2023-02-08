using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance {
		get { 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameManager>();
				_instance.OnCreateInstance ();
			}
			return _instance;
		}
	}

	private static GameManager _instance;

	public Color[] teams;

	internal Outpost[] outposts;

	private void OnCreateInstance () {
		outposts = GetComponentsInChildren<Outpost> ();
		Debug.Log("The size of the outposts is " + outposts.Length);
	}
}

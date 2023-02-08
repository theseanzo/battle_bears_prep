using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public AIController aiPrefab;

	public float spawnRange = 3;
	public float interval;
	private float timer = Mathf.Infinity;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > interval)
		{
			timer = 0;
			Spawn();
		}
	}

	void Spawn()
	{
		Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
		AIController ai = Instantiate(aiPrefab, spawnPos, transform.rotation) as AIController;
		ai.team = Random.Range(0, GameManager.instance.teams.Length);
	}
}


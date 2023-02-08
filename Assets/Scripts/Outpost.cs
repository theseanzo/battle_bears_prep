using UnityEngine;
using System.Collections;

public class Outpost : MonoBehaviour
{

	public float valueIncrease = 0.02f;
	public float flagTop = 7;
	public float flagBottom = 1;
	public float scoreInterval;

	internal int team = -1;
	internal float currentValue;

	private SkinnedMeshRenderer flag;

	private float timer;



	void Start()
	{
		flag = GetComponentInChildren<SkinnedMeshRenderer>();
	}

	void Update()
	{
		if (team != -1)
		{
			Color teamColor = GameManager.instance.teams [team];
			flag.material.color = Color.Lerp(Color.white, teamColor, currentValue);
			flag.transform.parent.localPosition = new Vector3(0, Mathf.Lerp(flagBottom, flagTop, currentValue));
			if (currentValue == 1)
			{
				timer += Time.deltaTime;
				if (timer >= scoreInterval)
				{
					timer = 0;
					ScoreManager.instance.scores [team]++;
				}
			}
		}
	}

	private void OnTriggerStay(Collider coll)
	{
		Unit unit = coll.GetComponent<Unit>();
		if (unit != null)
		{
			if (unit.team == team)
			{
				currentValue += valueIncrease;

				if (currentValue >= 1)
				{
					currentValue = 1;
				}
			} else
			{
				currentValue -= valueIncrease;

				if (currentValue <= 0)
				{
					currentValue = -currentValue;
					team = unit.team;
				}
			}
		}
	}
}

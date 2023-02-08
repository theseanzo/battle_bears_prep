using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : Unit
{
	
	public Vector3 aimOffset = new Vector3(0, 1.5f, 0);
	public float lookDistance = 10;
	public float shootInterval = 0.5f;

	private UnityEngine.AI.NavMeshAgent agent;

	private enum State
	{
		Idle,
		MovingToOutpost,
		Chasing
	}

	private State currentState;

	private Outpost currentOutpost;
	private Unit currentEnemy;

	private void SetState(State newState)
	{
		currentState = newState;
		StopAllCoroutines();
		switch (currentState)
		{
			case State.Idle:
				StartCoroutine(OnIdle());
				break;
			case State.MovingToOutpost:
				StartCoroutine(OnMovingToOutpost());
				break;
			case State.Chasing:
				StartCoroutine(OnChasing());
				break;
		}
	}

	// Use this for initialization
	protected override void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		animator.SetFloat("VerticalSpeed", agent.velocity.magnitude);
	}

	public override void OnHit(Unit attacker)
	{
		if (currentEnemy == null)
		{
			currentEnemy = attacker;
			SetState(State.Chasing);
		}
		base.OnHit(attacker);
	}

	private IEnumerator OnIdle()
	{
		while (currentOutpost == null)
		{
			LookForOutposts();
			yield return null;
		}
		//currentOutpost is no longer null here
		SetState(State.MovingToOutpost);
	}

	private IEnumerator OnMovingToOutpost()
	{
		agent.SetDestination(currentOutpost.transform.position);

		while (!(currentOutpost.team == team && currentOutpost.currentValue == 1))
		{
			LookForEnemies();
			yield return null;
		}

		currentOutpost = null;
		SetState(State.Idle);
	}

	private IEnumerator OnChasing()
	{
		agent.ResetPath();

		float shootTimer = 0;
		while (currentEnemy.isAlive)
		{
			shootTimer += Time.deltaTime;

			float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, this.transform.position);

			if (distanceToEnemy > lookDistance || !CanSee(currentEnemy.transform, currentEnemy.transform.position + aimOffset))
			{
				agent.SetDestination(currentEnemy.transform.position);
			} else if (shootTimer >= shootInterval)
			{
				agent.ResetPath();
				shootTimer = 0;
				ShootAt(currentEnemy.transform);
				ShowLasers(currentEnemy.transform.position + aimOffset);
			}


			yield return null;
		}
		currentEnemy = null;
		SetState(State.Idle);
	}

	void OnAnimatorIK(int layerIndex)
	{
		if (currentEnemy != null)
		{
			animator.SetLookAtPosition(currentEnemy.transform.position + aimOffset);
			animator.SetLookAtWeight(1);
		} 
	}

	private void LookForOutposts()
	{
		int r = Random.Range(0, GameManager.instance.outposts.Length);
		currentOutpost = GameManager.instance.outposts [r];
	}

	private void LookForEnemies()
	{
		Collider[] surroundingColliders = Physics.OverlapSphere(this.transform.position, lookDistance);
		foreach (Collider coll in surroundingColliders)
		{
			Unit unit = coll.GetComponent<Unit>();
			if (unit != null && unit != this && unit.team != team && unit.isAlive && CanSee(unit.transform, unit.transform.position + aimOffset))
			{
				currentEnemy = unit;
				SetState(State.Chasing);
				return;
			}
		}
	}

	protected override void Respawn()
	{
		base.Respawn();
		SetState(State.Idle);
	}

	protected override void Die()
	{
		base.Die();
		currentEnemy = null;
		StopAllCoroutines();
		agent.ResetPath();
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}
	
}








 


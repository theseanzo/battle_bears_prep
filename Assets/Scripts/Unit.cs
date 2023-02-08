using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Unit : MonoBehaviour
{
	public Laser laserPrefab;

	public int team;
	public float respawnTime = 2f;
	public int fullHealth = 100;
	public int damage = 10;
	public float viewAngle = 80;

	internal bool isAlive;

	protected Animator animator;

	protected Rigidbody rb;

	private Color myColor;
	private Vector3 startPos;
	private int health;
	private Eye[] eyes = new Eye[2];

	private const float RAYCAST_LENGTH = 0.3f;


	protected virtual void Start()
	{
		eyes = GetComponentsInChildren<Eye>();
	
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		myColor = GameManager.instance.teams [team];
		transform.Find("Teddy_Body").GetComponent<SkinnedMeshRenderer>().material.color = myColor;
		startPos = this.transform.position;

		Respawn();
	}

	protected virtual void Update()
	{
	}

	public virtual void OnHit(Unit attacker)
	{
		Debug.Log("You hit me");
		health -= attacker.damage;
		if (health <= 0)
		{
			Die();
		}
	}

	protected bool CanSee(Transform target, Vector3 targetPosition)
	{
		Vector3 startPos = (eyes [0].transform.position + eyes [1].transform.position) / 2;
		Vector3 dir = targetPosition - startPos;

		if (Vector3.Angle(transform.forward, dir) > viewAngle)
		{
			return false;
		}

		Ray ray = new Ray(startPos, dir);

		LayerMask mask = ~LayerMask.GetMask("Outpost");

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
		{
			if (hit.transform != target)
			{
				//Debug.Log(hit.transform.name);
				return false;
			}
		}

		return true;
	}

	protected void ShootAt(Transform target)
	{
		Unit unit = target.GetComponent<Unit>();
		if (unit != null)
		{
			unit.OnHit(this);
		}
	}

	protected void ShowLasers(Vector3 targetPosition)
	{
		foreach (Eye eye in eyes)
		{
			Laser laser = Instantiate(laserPrefab) as Laser;
			laser.Init(myColor, eye.transform.position, targetPosition);
		}
	}

	protected virtual void Respawn()
	{
		isAlive = true;
		health = fullHealth;
		animator.SetBool("Die", false);
		gameObject.layer = LayerMask.NameToLayer("LiveTeddy");
		this.transform.position = startPos;
	}

	protected virtual void Die()
	{
		if (!isAlive)
			return;
		isAlive = false;
		gameObject.layer = LayerMask.NameToLayer("DeadTeddy");
		animator.SetBool("Die", true);
		Invoke("Respawn", respawnTime);
	}

	protected bool IsGrounded()
	{
		Vector3 origin = transform.position;
		origin.y += RAYCAST_LENGTH * 0.5f;
		LayerMask mask = LayerMask.GetMask("Terrain");
		return Physics.Raycast(origin, Vector3.down, RAYCAST_LENGTH, mask);
	}

}













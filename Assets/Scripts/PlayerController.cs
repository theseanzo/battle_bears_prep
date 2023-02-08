using UnityEngine;
using System.Collections;

public class PlayerController : Unit
{
	private Camera playerCam;
	private Transform camContainer;
	public float speed = 5;
	public float mouseXSensitivity = 1;
	public float mouseYSensitivity = 1;
	public float jumpHeight = 15;

	private const float ANIMATOR_SMOOTHING = 0.4f;
	private const float DISTANCE_LASER_IF_NO_HIT = 300;

	private Vector3 animatorInput;

	protected override void Start()
	{
		base.Start();
		playerCam = GetComponentInChildren<Camera>();
		camContainer = playerCam.transform.parent;
	}

	protected override void Update()
	{
		base.Update();
		if (Time.timeScale == 0)
			return;
		
		camContainer.Rotate(Input.GetAxis("Mouse Y") * mouseYSensitivity, 0, 0);

		if (!isAlive)
			return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 input = new Vector3(horizontal, 0, vertical).normalized * speed;
		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
		{
			input.y = jumpHeight;
			animator.SetTrigger("Jump");
		} else
		{
			input.y = GetComponent<Rigidbody>().velocity.y;
		}

		GetComponent<Rigidbody>().velocity = transform.TransformVector(input);

		animatorInput = Vector3.Lerp(animatorInput, input, ANIMATOR_SMOOTHING);
		animator.SetFloat("HorizontalSpeed", animatorInput.x);
		animator.SetFloat("VerticalSpeed", animatorInput.z);


		float rotationX = Input.GetAxis("Mouse X") * mouseXSensitivity;
		this.transform.Rotate(0, rotationX, 0);

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

			LayerMask mask = ~LayerMask.GetMask("Teddy", "Outpost");

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
			{
				if (CanSee(hit.transform, hit.point))
				{
					ShootAt(hit.transform);
					ShowLasers(hit.point);
				}
			} else
			{
				Vector3 targetPos = playerCam.transform.position + playerCam.transform.forward * DISTANCE_LASER_IF_NO_HIT;
				ShowLasers(targetPos);
			}
		}
	}


}



















using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{

	public float lifeTime = 0.05f;

	private LineRenderer line;

	void Awake()
	{
		line = GetComponent<LineRenderer>();
	}

	public void Init(Color c, Vector3 start, Vector3 end)
	{
		line.SetColors(c, c);
		line.SetPosition(0, start);
		line.SetPosition(1, end);
		Invoke("DestroyMe", lifeTime);
	}

	private void DestroyMe()
	{
		Destroy(this.gameObject);
	}
}

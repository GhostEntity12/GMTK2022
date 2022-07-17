using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRotate : MonoBehaviour
{
	[SerializeField] bool doBob;
	[SerializeField] float rotSpeed = 10;
	[SerializeField] float bobSpeed = 10;
	[SerializeField] float bobHeight = 0.1f;
	float offset;
	float cacheY;

	private void Start()
	{
		offset = Random.Range(1f, 5f);
		cacheY = transform.position.y;
	}
	void Update()
	{
		transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.World);
		if (doBob)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, cacheY + bobHeight * Mathf.Sin(bobSpeed * Time.time + offset), transform.localPosition.z);
		}
	}
}

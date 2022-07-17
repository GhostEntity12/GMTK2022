using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRotate : MonoBehaviour
{
	[SerializeField] bool doBob;
    [SerializeField] float rotSpeed = 10;
    [SerializeField] float bobSpeed = 10;
    Vector3 cachePos;
	float offset;


	private void Start()
	{
		offset = Random.Range(1f, 5f);
		cachePos = transform.position;
	}
	void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.World);
		if (doBob)
		{
			transform.position = cachePos + Vector3.up * Mathf.Sin(Time.time + offset) * bobSpeed;
		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promo : MonoBehaviour
{
	[SerializeField] GameObject die;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	[ContextMenu("Spawn")]
	void Spawn()
	{
		Vector3 holder = Vector3.zero;
		for (int i = 0; i < 15; i++)
		{
			holder = new Vector3(0, holder.y, holder.z);
			for (int j = 0; j < 60; j++)
			{
				GameObject die = Instantiate(this.die, holder, Quaternion.identity);
				die.transform.rotation = Quaternion.Euler(Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90);
				holder += Vector3.left;
			}
			holder += Vector3.up;
		}

	}
}

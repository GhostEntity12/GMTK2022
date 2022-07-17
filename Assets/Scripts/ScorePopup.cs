using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : Poolable
{
	[SerializeField] float raiseSpeed;
	TextMeshProUGUI number;
	float lifeSpan;

	// Start is called before the first frame update
	void Awake()
	{
		number = GetComponent<TextMeshProUGUI>();
		lifeSpan = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (lifeSpan > 0)
		{
			transform.position += Vector3.up * raiseSpeed * Time.deltaTime;
			lifeSpan -= Time.deltaTime;
			number.color = new Color(number.color.r, number.color.g, number.color.b, Mathf.Min(1, 2 * lifeSpan));
		}
		else
		{
			ReturnToPool();
		}
	}

	public void SetLifespan(float value) => lifeSpan = value;
	public void SetAmount(int value)
	{
		number.text = value.ToString();
		number.fontSize = Mathf.Lerp(30, 120, value / 1000); 
	}
}

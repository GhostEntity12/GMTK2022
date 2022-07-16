using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	TextMeshProUGUI scoreText;

	int score;
	int tempScore;

	[SerializeField] int scoreIncreaseSpeed = 27;

	private void Awake()
	{
		scoreText = GetComponent<TextMeshProUGUI>();
	}

	public void AddScore(int value)
	{
		score += value;
	}

	private void Update()
	{
		if (tempScore < score)
		{
			tempScore += scoreIncreaseSpeed;
			tempScore = Mathf.Min(tempScore, score);

			scoreText.text = tempScore.ToString();
		}
	}

	[ContextMenu("Add Score")]
	void Add1500()
	{
		AddScore(1500);
	}
}

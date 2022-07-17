using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	TextMeshProUGUI scoreText;
	[SerializeField] int scoreIncreaseSpeed = 27;
	
	public int Score { get; private set; }
	int tempScore;


	private void Awake()
	{
		scoreText = GetComponent<TextMeshProUGUI>();
	}

	public void AddScore(int value)
	{
		Score += value;
	}

	private void Update()
	{
		if (tempScore < Score)
		{
			tempScore += scoreIncreaseSpeed;
			tempScore = Mathf.Min(tempScore, Score);

			scoreText.text = tempScore.ToString();
		}
	}
}

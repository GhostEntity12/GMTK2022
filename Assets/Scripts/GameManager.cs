using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
	public ScoreManager scoreManager;
	Selector selector;
	public GameSave Save { get; private set; }

	bool gameOver;
	[SerializeField] CanvasGroup gameOverCanvas;
	[SerializeField] TextMeshProUGUI scoreInfo;
	[SerializeField] TextMeshProUGUI highScoreText;

	const string highScoreMessage = "New High Score!";

	public AudioSource audioSource { get; private set; }

	[SerializeField] AudioClip newHighScore;

	int tempScore = 0;
	[SerializeField] int scoreIncreaseSpeed = 579;

	int coinsEarned = 0;
	float tempCoins = 0;
	float coinIncreaseSpeed = 0.1f;

	private void Start()
	{
		selector = FindObjectOfType<Selector>();
		Save = GetComponent<GameSave>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (gameOver)
		{
			gameOverCanvas.alpha += Time.deltaTime * 2f;

			tempScore = Mathf.Min(tempScore + scoreIncreaseSpeed, scoreManager.Score);
			tempCoins = Mathf.Min(tempCoins + coinIncreaseSpeed, coinsEarned);
			scoreInfo.text = $"<b><line-height=0%><align=left>Score:</b>\n<line-height=100%><align=right>{tempScore}\n<b><line-height=0%><align=left>Coins Earned:</b>\n<line-height=100%><align=right>{Mathf.FloorToInt(tempCoins)}\n\n<b><line-height=0%><align=left>High Score:</b>\n<line-height=100%><align=right>{Save.GetHighScore()}";
		}
	}

	public void EndGame()
	{
		gameOverCanvas.interactable = true;
		gameOverCanvas.blocksRaycasts = true;
		selector.FinishDraw();
		selector.SetSelectable(false);

		gameOver = true;

		if (scoreManager.Score > Save.GetHighScore())
		{
			Save.SetHighScore(scoreManager.Score);
			highScoreText.text = highScoreMessage;
			audioSource.PlayOneShot(newHighScore);
		}

		coinsEarned = scoreManager.Score / 1000;
		scoreInfo.text = $"<b><line-height=0%><align=left>Score:</b>\n<line-height=100%><align=right>{0}\n<b><line-height=0%><align=left>Coins Earned:</b>\n<line-height=100%><align=right>{0}\n\n<b><line-height=0%><align=left>High Score:</b>\n<line-height=100%><align=right>{Save.GetHighScore()}";

		Save.AddCoins(coinsEarned);
	}

	public void PlayAgain() => SceneManager.LoadScene(2);
	public void ToMenu() => SceneManager.LoadScene(0);
}

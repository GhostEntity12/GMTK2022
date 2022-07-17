using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
	public ScoreManager scoreManager;
	Selector selector;
	public GameSave Save { get; private set; }

	bool gameOver;
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] TextMeshProUGUI scoreInfo;

	private void Start()
	{
		selector = FindObjectOfType<Selector>();
		Save = GetComponent<GameSave>();
	}

	public void EndGame()
	{
		selector.FinishDraw();
		selector.SetSelectable(false);

		gameOver = true;
		gameOverScreen.SetActive(true);

		int coins = scoreManager.Score / 1000;
		scoreInfo.text = $"<b><line-height=0%><align=left>Score:</b>\n<line-height=100%><align=right>{scoreManager.Score}\n<b><line-height=0%><align=left>Coins Earned:</b>\n<line-height=100%><align=right>{coins}";

		Save.AddCoins(coins);
	}

	void PlayAgain() => SceneManager.LoadScene(2);
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField] Image timerFill;
	[SerializeField] TextMeshProUGUI timerText;
	[SerializeField] float levelLength = 90;
	float timer;

	bool doTimer = true;

	// Start is called before the first frame update
	void Start()
	{
		timer = levelLength;
	}

	// Update is called once per frame
	void Update()
	{
		if (!doTimer) return;

		if (timer <= 0)
		{
			doTimer = false;
			GameManager.Instance.EndGame();
			return;
		}

		timer = Mathf.Max(0, timer - Time.deltaTime);
		timerText.text = $"{Mathf.Floor(timer / 60f):00}:{Mathf.Floor(timer % 60f):00}";
		timerFill.fillAmount = timer / levelLength;

	}
}

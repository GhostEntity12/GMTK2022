using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selector : MonoBehaviour
{
	const float LineOffset = 0.151f;
	Plane miscPlane = new Plane(Vector3.forward, LineOffset);

	Camera cam;
	LineRenderer line;

	bool drawing = false;
	/*
	 * 0 = Raindrop
	 * 1 = Coin
	 * 2 = Pear
	 * 3 = Explosion x3
	 * 4 = Explosion
	 * 5 = Flower
	 */
	int sideSelected = -1;

	readonly List<Die> dice = new List<Die>();

	[SerializeField] Spawner diceSpawner;

	[Header("Dice Info")]
	[SerializeField] LayerMask diceLayer;

	[Header("Line Info")]
	[SerializeField] float maxLength = 50;

	[SerializeField] BonusDie bonusPointDie;

	bool canSelect = true;

	public float PercentUsed => line.GetLength() / maxLength;

	[SerializeField] AudioClip selectSound;
	AudioSource audioSource;
	float scale = Mathf.Pow(2f, 1.0f / 12f);

	PopupPool popups;

	void Awake()
	{
		cam = Camera.main;
		line = GetComponent<LineRenderer>();
		audioSource = GetComponent<AudioSource>();
		popups = FindObjectOfType<PopupPool>();
	}

	void Start()
	{
		bonusPointDie.Roll();
	}

	void Update()
	{
		if (!canSelect) return;

		if (Input.GetMouseButtonDown(0) && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInit, Mathf.Infinity, diceLayer))
		{
			StartDraw(hitInit.transform.GetComponent<Die>());
		}

		if (Input.GetMouseButtonUp(0) || line.GetLength() > maxLength)
		{
			FinishDraw();
		}

		if (drawing && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitReocc, Mathf.Infinity, diceLayer))
		{
			Die selectedDie = hitReocc.transform.GetComponent<Die>();
			if (selectedDie.Side == sideSelected && !dice.Contains(selectedDie))
			{
				dice.Add(selectedDie);
				line.positionCount++;
				Vector3 diePos = selectedDie.transform.position;
				line.SetPosition(line.positionCount - 2, diePos - Vector3.forward * LineOffset);
				audioSource.pitch = Mathf.Pow(scale, dice.Count);
				audioSource.PlayOneShot(selectSound, 0.6f);
			}
		}

		if (drawing && miscPlane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out float hitBlock))
		{
			Vector3 point = cam.ScreenPointToRay(Input.mousePosition).GetPoint(hitBlock);
			line.SetPosition(line.positionCount - 1, point);
		}
	}

	void StartDraw(Die selectedDie)
	{
		drawing = true;

		sideSelected = selectedDie.Side;

		line.positionCount = 2;
		line.SetPosition(line.positionCount - 2, selectedDie.transform.position - Vector3.forward * LineOffset);
	}

	public void FinishDraw()
	{
		drawing = false;
		line.positionCount = 0;

		if (dice.Count >= 3)
		{
			List<Die> bonusDice = new List<Die>();
			Vector3 finalDiePos = dice[dice.Count - 1].transform.position;

			int scoreVal = dice.Count * 100 + (((dice.Count * dice.Count) - dice.Count) * 5);

			switch (sideSelected)
			{
				case 0: // Raindrop - vertical clear
					bonusDice = Physics.OverlapBox(finalDiePos, new Vector3(0.8f, 10, 1), Quaternion.identity, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
					break;
				case 1: // Coin - bonus points
					scoreVal = Mathf.FloorToInt(scoreVal * 1.4f);
					break;
				case 2: // Pears - pear clear
					bonusDice = diceSpawner.Pool.ActiveDice.Where(d => d.Side == sideSelected).ToList();
					break;
				case 3: // Mini explosions - mini circle clears
					foreach (Die die in dice)
					{
						bonusDice.AddRange(Physics.OverlapSphere(die.transform.position, 0.4f, diceLayer).Select(d => d.GetComponent<Die>()));
					}
					break;
				case 4: // Single Explosion - big circle clear
					bonusDice = Physics.OverlapSphere(finalDiePos, 1.25f, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
					break;
				case 5: // Flower - horizontal clear
					bonusDice = Physics.OverlapBox(finalDiePos, new Vector3(10f, 0.6f, 1), Quaternion.identity, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
					break;
				default:
					break;
			}

			// Affect all selected dice
			int i = 0;
			foreach (Die die in dice)
			{
				ScorePopup popup = popups.GetPooledObject() as ScorePopup;
				popup.transform.position = cam.WorldToScreenPoint(die.transform.position);
				popup.SetAmount((100 + i * 10) * (sideSelected == bonusPointDie.Side ? 2 : 1));
				popup.SetLifespan(1);
				die.Remove();
				i++;
			}

			bonusDice = bonusDice.Except(dice).ToList();
			if (bonusDice.Count > 0)
			{
				ScorePopup popup = popups.GetPooledObject() as ScorePopup;
				popup.transform.position = cam.WorldToScreenPoint(finalDiePos);
				popup.SetAmount(bonusDice.Count * 25 * (sideSelected == bonusPointDie.Side ? 2 : 1));
				popup.SetLifespan(1);
				foreach (Die die in bonusDice)
				{
					die.Remove();
				}
				scoreVal += bonusDice.Count * 25;
			}

			if (sideSelected == bonusPointDie.Side)
			{
				scoreVal *= 2;
			}

			GameManager.Instance.scoreManager.AddScore(scoreVal);
			diceSpawner.Spawn(dice.Count + bonusDice.Count);
			bonusPointDie.Roll();
		}

		sideSelected = -1;
		dice.Clear();
	}

	public void SetSelectable(bool canSelect) => this.canSelect = canSelect;
}

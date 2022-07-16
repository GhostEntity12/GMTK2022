using System.Collections;
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

	public float PercentUsed => line.GetLength() / maxLength;

	void Awake()
	{
		cam = Camera.main;
		line = GetComponent<LineRenderer>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInit, Mathf.Infinity, diceLayer))
		{
			drawing = true;

			Die selectedDie = hitInit.transform.GetComponent<Die>();
			sideSelected = selectedDie.Side;

			line.positionCount = 2;
			line.SetPosition(line.positionCount - 2, selectedDie.transform.position - Vector3.forward * LineOffset);
		}

		if (Input.GetMouseButtonUp(0) || line.GetLength() > maxLength)
		{
			drawing = false;

			line.positionCount = 0;


			if (dice.Count > 3)
			{
				int diceCount = dice.Count;

				int scoreVal = dice.Count * 100 + (((dice.Count * dice.Count) - dice.Count) * 5);

				Vector3 finalDiePos = dice[dice.Count - 1].transform.position;
				List<Die> bonusDice = new List<Die>();
				switch (sideSelected)
				{
					case 0:
						bonusDice = Physics.OverlapBox(finalDiePos, new Vector3(0.8f, 10, 1), Quaternion.identity, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
						break;
					case 1:
						scoreVal = Mathf.FloorToInt(scoreVal * 1.4f);
						break;
					case 2:
						Debug.Log(diceSpawner.Pool.ActiveDice.Count);
						bonusDice = diceSpawner.Pool.ActiveDice.Where(d => d.Side == sideSelected).ToList();
						break;
					case 3:
						foreach (Die die in dice)
						{
							bonusDice.AddRange(Physics.OverlapSphere(die.transform.position, 0.4f, diceLayer).Select(d => d.GetComponent<Die>()));
						}
						break;
					case 4:
						bonusDice = Physics.OverlapSphere(finalDiePos, 1.25f, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
						break;
					case 5:
						bonusDice = Physics.OverlapBox(finalDiePos, new Vector3(10f, 0.6f, 1), Quaternion.identity, diceLayer).Select(d => d.GetComponent<Die>()).ToList();
						break;
					default:
						break;
				}
				bonusDice = bonusDice.Except(dice).ToList();

				// Affect all selected dice
				foreach (Die die in dice)
				{
					die.Remove();
				}

				if (bonusDice.Count > 0)
				{
					foreach (Die die in bonusDice)
					{
						die.Remove();
					}
					scoreVal += bonusDice.Count * 25;
				}

				GameManager.Instance.scoreManager.AddScore(scoreVal);

				diceSpawner.Spawn(diceCount + bonusDice.Count);
			}

			sideSelected = -1;
			dice.Clear();
		}

		if (Input.GetMouseButton(0) && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitReocc, Mathf.Infinity, diceLayer))
		{
			Die selectedDie = hitReocc.transform.GetComponent<Die>();
			if (selectedDie.Side == sideSelected && !dice.Contains(selectedDie))
			{
				dice.Add(selectedDie);
				line.positionCount++;
				Vector3 diePos = selectedDie.transform.position;
				line.SetPosition(line.positionCount - 2, diePos - Vector3.forward * LineOffset);
			}
		}

		if (drawing && miscPlane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out float hitBlock))
		{
			Vector3 point = cam.ScreenPointToRay(Input.mousePosition).GetPoint(hitBlock);
			line.SetPosition(line.positionCount - 1, point);
		}
	}
}

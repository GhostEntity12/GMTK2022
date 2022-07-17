using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	public enum Rarity { Common, Uncommon, Rare, Legendary }
	public enum DiceBase { Blocky, FairyFloss, Galaxy, KingsDice, Nightmare, Pandas, Peach, Portal, Prismatic, Rainbow, Ripples, Storm, Count }
	public enum DiceIcon { Bloody, Blue, Clouds, Gold, Green, Lolly, Pink, Purple, Rainbow, Red, Shell, Yellow, Count }

	int index = 0;
	float targetRot;
	[SerializeField] float rotSpeed = 1f;

	[SerializeField] int[] costs = new int[] { 50, 100, 250, 500 };

	[SerializeField] Transform shopDiceTransform;
	[SerializeField] Transform shopDiceRotTarget;
	[SerializeField] Renderer[] shopDice;

	[SerializeField] Texture[] diceBases;
	[SerializeField] Texture[] diceIcons;
	Dictionary<DiceBase, Rarity> baseRarities = new Dictionary<DiceBase, Rarity>
	{
		{ DiceBase.Blocky, Rarity.Rare },
		{ DiceBase.FairyFloss, Rarity.Uncommon },
		{ DiceBase.Galaxy, Rarity.Uncommon },
		{ DiceBase.KingsDice, Rarity.Legendary},
		{ DiceBase.Nightmare, Rarity.Uncommon },
		{ DiceBase.Pandas, Rarity.Rare },
		{ DiceBase.Peach, Rarity.Uncommon },
		{ DiceBase.Portal, Rarity.Uncommon },
		{ DiceBase.Prismatic, Rarity.Uncommon},
		{ DiceBase.Rainbow, Rarity.Legendary},
		{ DiceBase.Ripples, Rarity.Uncommon},
		{ DiceBase.Storm, Rarity.Rare },
	};

	// Start is called before the first frame update
	void Start()
	{
		for (int i = -2; i < 3; i++)
		{
			SetDieBase(i);
		}
	}

	// Update is called once per frame
	void Update()
	{
		shopDiceRotTarget.eulerAngles = new Vector3(0, targetRot, 0);

		shopDiceTransform.rotation = Quaternion.Slerp(shopDiceTransform.rotation, shopDiceRotTarget.rotation, rotSpeed * Time.deltaTime);

		if (shopDiceTransform.rotation == shopDiceRotTarget.rotation && (targetRot == -360 || targetRot == 360))
		{
			targetRot = 0;
			shopDiceTransform.rotation = Quaternion.identity;
			shopDiceRotTarget.rotation = Quaternion.identity;
		}
	}

	public void IncrementIndex()
	{
		index = (index + 1 + shopDice.Length) % shopDice.Length;
		SetDieBase(index + 2);
		targetRot += 30;
	}
	public void DecrementIndex()
	{
		index = (index - 1 + shopDice.Length) % shopDice.Length;
		SetDieBase(index - 2);
		targetRot -= 30;
	}

	void SetDieBase(int index)
	{
		int b = index;
		int d = index;
		d = (d + shopDice.Length) % shopDice.Length;
		b = (b + diceBases.Length) % diceBases.Length;
		shopDice[d].material.SetTexture("_BaseTex", diceBases[b]);
	}
}

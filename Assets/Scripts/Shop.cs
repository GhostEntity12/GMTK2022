using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	public enum Rarity { Common, Uncommon, Rare, Legendary }
	public enum DiceBase { Blocky, FairyFloss, Galaxy, KingsDice, Nightmare, Pandas, Peach, Portal, Prismatic, Rainbow, Ripples, Storm, Count }
	public enum DiceIcon { Bloody, Blue, Clouds, Gold, Green, Lolly, Pink, Purple, Rainbow, Red, Shell, Yellow, Count }

	[SerializeField] int[] costs = new int[] { 50, 100, 250, 500 };
	[SerializeField] Renderer[] shopDice;
	[SerializeField] Texture[] diceBases;
	[SerializeField] Texture[] diceIcons;
	int index = 0;

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
			Debug.Log("did d");

			int b = i;
			int d = i;
			d = (d + shopDice.Length) % shopDice.Length;
			b = (b + diceBases.Length) % diceBases.Length;
			shopDice[d].material.SetTexture("_BaseTex", diceBases[b]);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}

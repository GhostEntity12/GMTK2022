using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
	public enum ShopType { Base, Icon, Ink }
	public enum Rarity { Common, Uncommon, Rare, Legendary }
	public enum DiceBase { Blocky, FairyFloss, Galaxy, Kings_Dice, Nightmare, Pandas, Peach, Portal, Prismatic, Rainbow, Ripples, Storm, Count }
	public enum DiceIcon { Bloody, Blue, Cheese, Clouds, Gold, Green, Lolly, Pink, Purple, Rainbow, Red, Shell, Yellow, Count }
	public enum DiceInk { Matte, Metallic, Count }

	int dieIndex = 0;
	int baseIndex = 0;
	int iconIndex = 0;
	int inkIndex = 0;

	GameSave save;

	ShopType shopType = ShopType.Base;

	[SerializeField] Material diceMaterial;

	[Header("Pricing")]
	[SerializeField] int[] costs = new int[] { 50, 100, 250, 500 };
	[SerializeField] TextMeshProUGUI coinCount;
	[SerializeField] TextMeshProUGUI dieInfo;

	[Header("Rotation")]
	[SerializeField] Transform shopDiceTransform;
	[SerializeField] Quaternion shopDiceRotTarget;
	[SerializeField] float rotSpeed = 15f;

	[Header("Models")]
	[SerializeField] Renderer[] shopDice;

	[Header("Textures")]
	[SerializeField] Texture[] diceBases;
	[SerializeField] Texture[] diceIcons;

	Dictionary<DiceBase, Rarity> baseRarities = new Dictionary<DiceBase, Rarity>
	{
		{ DiceBase.Blocky, Rarity.Rare },
		{ DiceBase.FairyFloss, Rarity.Uncommon },
		{ DiceBase.Galaxy, Rarity.Uncommon },
		{ DiceBase.Kings_Dice, Rarity.Legendary},
		{ DiceBase.Nightmare, Rarity.Uncommon },
		{ DiceBase.Pandas, Rarity.Rare },
		{ DiceBase.Peach, Rarity.Uncommon },
		{ DiceBase.Portal, Rarity.Uncommon },
		{ DiceBase.Prismatic, Rarity.Uncommon},
		{ DiceBase.Rainbow, Rarity.Legendary},
		{ DiceBase.Ripples, Rarity.Uncommon},
		{ DiceBase.Storm, Rarity.Rare },
	};
	Dictionary<DiceIcon, Rarity> iconRarities = new Dictionary<DiceIcon, Rarity>
	{
		{ DiceIcon.Bloody, Rarity.Uncommon },
		{ DiceIcon.Blue, Rarity.Common},
		{ DiceIcon.Cheese, Rarity.Uncommon },
		{ DiceIcon.Clouds, Rarity.Uncommon },
		{ DiceIcon.Gold, Rarity.Rare },
		{ DiceIcon.Green, Rarity.Common},
		{ DiceIcon.Lolly, Rarity.Uncommon},
		{ DiceIcon.Pink, Rarity.Common},
		{ DiceIcon.Purple, Rarity.Common},
		{ DiceIcon.Rainbow, Rarity.Legendary},
		{ DiceIcon.Red, Rarity.Common},
		{ DiceIcon.Shell, Rarity.Uncommon},
		{ DiceIcon.Yellow, Rarity.Common},
	};
	Dictionary<DiceInk, Rarity> inkRarities = new Dictionary<DiceInk, Rarity>
	{
		{ DiceInk.Matte, Rarity.Common },
		{ DiceInk.Metallic, Rarity.Legendary },
	};

	// Start is called before the first frame update
	void Start()
	{
		save = GetComponent<GameSave>();
		save.Read();
		coinCount.text = $"<sprite=\"Icons\" name=\"Coin\"> {save.Coins}";

		SetShopType(ShopType.Base);

		SetInfoString();
	}

	// Update is called once per frame
	void Update()
	{
		shopDiceTransform.rotation = Quaternion.Slerp(shopDiceTransform.rotation, shopDiceRotTarget, rotSpeed * Time.deltaTime);

		if (shopDiceTransform.rotation == shopDiceRotTarget && shopDiceRotTarget == Quaternion.identity)
		{
			shopDiceTransform.rotation = Quaternion.identity;
			shopDiceRotTarget = Quaternion.identity;
		}
	}

	public void IncrementIndex()
	{
		dieIndex = dieIndex.Increment(shopDice.Length);
		switch (shopType)
		{
			case ShopType.Base:
				baseIndex = baseIndex.Increment(diceBases.Length);
				SetDieBase(dieIndex + 2, baseIndex + 2);
				break;
			case ShopType.Icon:
				iconIndex = iconIndex.Increment(diceIcons.Length);
				SetDieIcon(dieIndex + 2, iconIndex + 2);
				break;
			case ShopType.Ink:
				inkIndex = inkIndex.Increment((int)DiceInk.Count);
				SetDieInk(dieIndex + 2, inkIndex + 2);
				break;
			default:
				break;
		}
		shopDiceRotTarget = Quaternion.Euler(0, shopDiceRotTarget.eulerAngles.y + 30, 0);
		SetInfoString();
	}
	public void DecrementIndex()
	{
		dieIndex = dieIndex.Decrement(shopDice.Length);
		switch (shopType)
		{
			case ShopType.Base:
				baseIndex = baseIndex.Decrement(diceBases.Length);
				SetDieBase(dieIndex - 2, baseIndex - 2);
				break;
			case ShopType.Icon:
				iconIndex = iconIndex.Decrement(diceIcons.Length);
				SetDieIcon(dieIndex - 2, iconIndex - 2);
				break;
			case ShopType.Ink:
				inkIndex = inkIndex.Decrement((int)DiceInk.Count);
				SetDieInk(dieIndex - 2, inkIndex - 2);
				break;
			default:
				break;
		}
		shopDiceRotTarget = Quaternion.Euler(0, shopDiceRotTarget.eulerAngles.y - 30, 0);
		SetInfoString();
	}

	void SetInfoString() => dieInfo.text = shopType switch
	{
		ShopType.Base => $"{((DiceBase)baseIndex).ToString().Replace('_', ' ')} - <sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)baseRarities[(DiceBase)baseIndex]]}",
		ShopType.Icon => $"{((DiceIcon)iconIndex).ToString().Replace('_', ' ')} - <sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)iconRarities[(DiceIcon)iconIndex]]}",
		ShopType.Ink => $"{((DiceInk)inkIndex).ToString().Replace('_', ' ')} - <sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)inkRarities[(DiceInk)inkIndex]]}",
		_ => throw new System.Exception()
	};

	void SetDieBase(int dieIndex, int baseIndex)
	{
		dieIndex = (dieIndex + shopDice.Length) % shopDice.Length;
		baseIndex = (baseIndex + diceBases.Length) % diceBases.Length;
		shopDice[dieIndex].material.SetTexture("_BaseTex", diceBases[baseIndex]);
	}
	void SetDieIcon(int dieIndex, int iconIndex)
	{
		dieIndex = (dieIndex + shopDice.Length) % shopDice.Length;
		iconIndex = (iconIndex + diceIcons.Length) % diceIcons.Length;
		shopDice[dieIndex].material.SetTexture("_IconTex", diceIcons[iconIndex]);
	}
	void SetDieInk(int dieIndex, int inkIndex)
	{
		dieIndex = (dieIndex + shopDice.Length) % shopDice.Length;
		inkIndex = (inkIndex + (int)DiceInk.Count) % (int)DiceInk.Count;
		shopDice[dieIndex].material.SetFloat("_ShinyIcons", inkIndex == (int)DiceInk.Matte ? 0 : 1);
	}

	public void PurchaseEquip()
	{
		switch (shopType)
		{
			case ShopType.Base:
				DiceBase body = (DiceBase)baseIndex;
				if (save.BaseUnlocked(body))
					EquipBody(body);
				else
					PurchaseBody(body);
				break;
			case ShopType.Icon:
				DiceIcon icon = (DiceIcon)iconIndex;
				if (save.IconUnlocked(icon))
					EquipIcon(icon);
				else
					PurchaseIcon(icon);
				break;
			case ShopType.Ink:
				DiceInk ink = (DiceInk)inkIndex;
				if (save.InkUnlocked(ink))
					EquipInk(ink);
				else
					PurchaseInk(ink);
				break;
			default:
				break;
		}
		coinCount.text = $"<sprite=\"Icons\" name=\"Coin\"> {save.Coins}";
		save.Write();
	}

	void PurchaseBody(DiceBase body)
	{
		if (save.UseCoins(costs[(int)baseRarities[body]]))
		{
			save.UnlockBase(body);
		}
	}
	void PurchaseIcon(DiceIcon icon)
	{

		if (save.UseCoins(costs[(int)iconRarities[icon]]))
		{
			save.UnlockIcon(icon);
		}
	}
	void PurchaseInk(DiceInk ink)
	{
		if (save.UseCoins(costs[(int)inkRarities[ink]]))
		{
			save.UnlockInk(ink);
		}
	}

	void EquipBody(DiceBase body)
	{
		diceMaterial.SetTexture("_BaseTex", diceBases[(int)body]);
		save.SetBody(body);
	}
	void EquipIcon(DiceIcon icon)
	{
		diceMaterial.SetTexture("_IconTex", diceBases[(int)icon]);
		save.SetIcon(icon);
	}
	void EquipInk(DiceInk ink)
	{
		diceMaterial.SetFloat("_ShinyIcons", ink == DiceInk.Matte ? 0 : 1);
		save.SetInk(ink);
	}

	public void SetShopType(int i) => SetShopType((ShopType)i);
	void SetShopType(ShopType type)
	{
		shopType = type;
		for (int i = -2; i < 3; i++)
		{
			switch (shopType)
			{
				case ShopType.Base:
					SetDieBase(i + dieIndex, i);
					break;
				case ShopType.Icon:
					SetDieIcon(i + dieIndex, i);
					break;
				case ShopType.Ink:
					SetDieInk(i + dieIndex, i);
					break;
				default:
					break;
			}
		}
		for (int i = 0; i < shopDice.Length; i++)
		{
			switch (shopType)
			{
				case ShopType.Base:
					SetDieIcon(i, save.GetIcon());
					SetDieInk(i, save.GetInk());
					break;
				case ShopType.Icon:
					SetDieBase(i, save.GetBody());
					SetDieInk(i, save.GetInk());
					break;
				case ShopType.Ink:
					SetDieBase(i, save.GetBody());
					SetDieIcon(i, save.GetIcon());
					break;
				default:
					break;
			}
		}
		SetInfoString();
	}
}

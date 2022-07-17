using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public enum ShopType { Base, Icon, Ink }
	public enum Rarity { Common, Uncommon, Rare, Legendary }

	public enum DiceBase
	{
		Black,
		White,
		Red,
		Yellow,
		Green,
		Blue,
		Purple,
		Pink,
		Bubblegum,
		Fairy_Floss,
		Galaxy,
		Mossy,
		Nightmare,
		Peach,
		Portal,
		Prismatic,
		Ripples,
		Witchy,
		Blocky,
		Cheese,
		Pandas,
		Storm,
		Weather,
		King_Dice,
		Rainbow,
		Stained_Glass,
		Count
	}
	Dictionary<DiceBase, Rarity> baseRarities = new Dictionary<DiceBase, Rarity>
	{
		{ DiceBase.Black, Rarity.Common},
		{ DiceBase.White, Rarity.Common},
		{ DiceBase.Red, Rarity.Common},
		{ DiceBase.Yellow, Rarity.Common},
		{ DiceBase.Green, Rarity.Common},
		{ DiceBase.Blue, Rarity.Common},
		{ DiceBase.Purple, Rarity.Common},
		{ DiceBase.Pink, Rarity.Common},
		{ DiceBase.Bubblegum, Rarity.Uncommon },
		{ DiceBase.Fairy_Floss, Rarity.Uncommon },
		{ DiceBase.Galaxy, Rarity.Uncommon },
		{ DiceBase.Mossy, Rarity.Uncommon },
		{ DiceBase.Nightmare, Rarity.Uncommon },
		{ DiceBase.Peach, Rarity.Uncommon },
		{ DiceBase.Portal, Rarity.Uncommon },
		{ DiceBase.Prismatic, Rarity.Uncommon},
		{ DiceBase.Ripples, Rarity.Uncommon},
		{ DiceBase.Witchy, Rarity.Uncommon},
		{ DiceBase.Blocky, Rarity.Rare },
		{ DiceBase.Cheese, Rarity.Rare },
		{ DiceBase.Pandas, Rarity.Rare },
		{ DiceBase.Storm, Rarity.Rare },
		{ DiceBase.Weather, Rarity.Rare },
		{ DiceBase.King_Dice, Rarity.Legendary},
		{ DiceBase.Rainbow, Rarity.Legendary},
		{ DiceBase.Stained_Glass, Rarity.Legendary},
	};

	public enum DiceIcon
	{
		Colors,
		Black,
		White,
		Red,
		Yellow,
		Green,
		Blue,
		Purple,
		Pink,
		Bloody,
		Cheese,
		Clouds,
		Lolly,
		Shell,
		Gold,
		Rainbow,
		Count
	}
	Dictionary<DiceIcon, Rarity> iconRarities = new Dictionary<DiceIcon, Rarity>
	{
		{ DiceIcon.Colors, Rarity.Common},
		{ DiceIcon.Black, Rarity.Common},
		{ DiceIcon.White, Rarity.Common},
		{ DiceIcon.Red, Rarity.Common},
		{ DiceIcon.Yellow, Rarity.Common},
		{ DiceIcon.Green, Rarity.Common},
		{ DiceIcon.Blue, Rarity.Common},
		{ DiceIcon.Purple, Rarity.Common},
		{ DiceIcon.Pink, Rarity.Common},
		{ DiceIcon.Bloody, Rarity.Uncommon },
		{ DiceIcon.Cheese, Rarity.Uncommon },
		{ DiceIcon.Clouds, Rarity.Uncommon },
		{ DiceIcon.Lolly, Rarity.Uncommon},
		{ DiceIcon.Shell, Rarity.Uncommon},
		{ DiceIcon.Gold, Rarity.Rare },
		{ DiceIcon.Rainbow, Rarity.Legendary},
	};

	public enum DiceInk { Matte, Metallic, Count }
	Dictionary<DiceInk, Rarity> inkRarities = new Dictionary<DiceInk, Rarity>
	{
		{ DiceInk.Matte, Rarity.Common },
		{ DiceInk.Metallic, Rarity.Legendary },
	};

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
	[SerializeField] Button purchaseButton;
	[SerializeField] TextMeshProUGUI purchaseText;

	[Header("Rotation")]
	[SerializeField] Transform shopDiceTransform;
	[SerializeField] Quaternion shopDiceRotTarget;
	[SerializeField] float rotSpeed = 15f;

	[Header("Models")]
	[SerializeField] Renderer[] shopDice;

	[Header("Textures")]
	[SerializeField] Texture[] diceBases;
	[SerializeField] Texture[] diceIcons;


	[Header("Audio")]
	[SerializeField] AudioClip purchase;
	AudioSource audioSource;

	// Start is called before the first frame update
	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		save = GetComponent<GameSave>();
	}
	void Start()
	{
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

	void SetInfoString()
	{
		dieInfo.text = shopType switch
		{
			ShopType.Base => $"{((DiceBase)baseIndex).ToString().Replace('_', ' ')}\n<sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)baseRarities[(DiceBase)baseIndex]]}",
			ShopType.Icon => $"{((DiceIcon)iconIndex).ToString().Replace('_', ' ')}\n<sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)iconRarities[(DiceIcon)iconIndex]]}",
			ShopType.Ink => $"{((DiceInk)inkIndex).ToString().Replace('_', ' ')}\n<sprite=\"Icons\" name=\"Coin\" color=#000000> {costs[(int)inkRarities[(DiceInk)inkIndex]]}",
			_ => throw new System.Exception()
		};

		switch (shopType)
		{
			case ShopType.Base:
				if (save.Unlocked((DiceBase)baseIndex))
				{
					purchaseText.text = "Equip";
					purchaseButton.interactable = save.GetBody() != baseIndex;
				}
				else
				{
					purchaseText.text = "Purchase";
					purchaseButton.interactable = costs[(int)baseRarities[(DiceBase)baseIndex]] < save.Coins;
				}
				break;
			case ShopType.Icon:
				if (save.Unlocked((DiceIcon)iconIndex))
				{
					purchaseText.text = "Equip";
					purchaseButton.interactable = save.GetIcon() != iconIndex;
				}
				else
				{
					purchaseText.text = "Purchase";
					purchaseButton.interactable = costs[(int)iconRarities[(DiceIcon)iconIndex]] < save.Coins;
				}
				break;
			case ShopType.Ink:
				if (save.Unlocked((DiceIcon)iconIndex))
				{
					purchaseText.text = "Equip";
					purchaseButton.interactable = save.GetInk() != inkIndex;
				}
				else
				{
					purchaseText.text = "Purchase";
					purchaseButton.interactable = costs[(int)inkRarities[(DiceInk)inkIndex]] < save.Coins;
				}
				break;
			default:
				break;
		}
	}

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
				if (save.Unlocked(body))
					EquipBody(body);
				else
					PurchaseBody(body);
				break;
			case ShopType.Icon:
				DiceIcon icon = (DiceIcon)iconIndex;
				if (save.Unlocked(icon))
					EquipIcon(icon);
				else
					PurchaseIcon(icon);
				break;
			case ShopType.Ink:
				DiceInk ink = (DiceInk)inkIndex;
				if (save.Unlocked(ink))
					EquipInk(ink);
				else
					PurchaseInk(ink);
				break;
			default:
				break;
		}
		coinCount.text = $"<sprite=\"Icons\" name=\"Coin\"> {save.Coins}";
		save.Write();
		SetInfoString();
	}

	void PurchaseBody(DiceBase body)
	{
		if (save.UseCoins(costs[(int)baseRarities[body]]))
		{
			save.UnlockBase(body);
			audioSource.PlayOneShot(purchase);
		}
	}
	void PurchaseIcon(DiceIcon icon)
	{

		if (save.UseCoins(costs[(int)iconRarities[icon]]))
		{
			save.UnlockIcon(icon);
			audioSource.PlayOneShot(purchase);
		}
	}
	void PurchaseInk(DiceInk ink)
	{
		if (save.UseCoins(costs[(int)inkRarities[ink]]))
		{
			save.UnlockInk(ink);
			audioSource.PlayOneShot(purchase);
		}
	}

	void EquipBody(DiceBase body)
	{
		diceMaterial.SetTexture("_BaseTex", diceBases[(int)body]);
		save.SetBody(body);
	}
	void EquipIcon(DiceIcon icon)
	{
		diceMaterial.SetTexture("_IconTex", diceIcons[(int)icon]);
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
		baseIndex = 0;
		iconIndex = 0;
		inkIndex = 0;
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

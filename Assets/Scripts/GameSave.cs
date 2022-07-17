using System;
using UnityEngine;
using System.IO;
using System.Text;

public class GameSave : MonoBehaviour
{
	public enum DiceBase { FairyFloss, Galaxy, Blocky, KingsDice, Nightmare, Pandas, Peach, Portal, Prismatic, Rainbow, Ripples, Storm, Count }
	public enum DiceIcon { Bloody, Blue, Clouds, Gold, Green, Lolly, Pink, Purple, Rainbow, Red, Shell, Yellow, Count }

	public int Coins => save.coins;

	private SaveData save;
	string path;
	const string Key = "GMTK2022";

	// Start is called before the first frame update
	void Start()
	{
		path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.LastIndexOf(Application.productName));
		Debug.Log(path);
	}

	public void AddCoins(int amount)
	{
		save.coins += amount;
	}

	public bool UseCoins(int amount)
	{
		if (save.coins <= amount) return false;

		save.coins -= amount;
		return true;
	}

	public void UnlockBase(DiceBase unlock) => save.baseUnlocks[(int)unlock] = true;
	public void UnlockIcon(DiceIcon unlock) => save.iconUnlocks[(int)unlock] = true;
	public void UnlockShinyIcons() => save.iconShiny = true;


	[ContextMenu("Save File")]
	public void Write()
	{
		string jsonRaw = JsonUtility.ToJson(save);
		print(jsonRaw);
		string encodedData = EncryptDecrypt(jsonRaw);
		print(encodedData);
		File.WriteAllText(path + Path.DirectorySeparatorChar + "save.dat", encodedData);
	}

	[ContextMenu("Read File")]
	public void Read()
	{
		string encodedData = File.ReadAllText(path + Path.DirectorySeparatorChar + "save.dat");
		print(encodedData);
		string jsonRaw = EncryptDecrypt(encodedData);
		print(jsonRaw);
		save = (SaveData)JsonUtility.FromJson(jsonRaw, typeof(SaveData));
	}

	string EncryptDecrypt(string data)
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < data.Length; i++)
		{
			sb.Append((char)(data[i] ^ Key[i % Key.Length]));
		}

		return sb.ToString();
	}
}

[Serializable]
public class SaveData
{
	public int coins;
	public bool[] baseUnlocks = new bool[(int)GameSave.DiceBase.Count];
	public bool[] iconUnlocks = new bool[(int)GameSave.DiceIcon.Count];
	public bool iconShiny;
}
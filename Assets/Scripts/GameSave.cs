using System;
using System.IO;
using System.Text;
using UnityEngine;
public class GameSave : MonoBehaviour
{

	public int Coins => save.coins;

	private SaveData save;
	string path;
	const string Key = "GMTK2022";

	// Start is called before the first frame update
	void Start()
	{
		path = Application.persistentDataPath;
		Read();
	}

	public void AddCoins(int amount)
	{
		save.coins += amount;
		Write();
	}

	public bool UseCoins(int amount)
	{
		if (save.coins <= amount) return false;

		save.coins -= amount;
		Write();

		return true;
	}

	public void UnlockBase(Shop.DiceBase unlock) => save.bases.unlocks[(int)unlock] = true;
	public void UnlockIcon(Shop.DiceIcon unlock) => save.icons.unlocks[(int)unlock] = true;
	public void UnlockInk(Shop.DiceInk unlock) => save.inks.unlocks[(int)unlock] = true;

	public bool Unlocked(Shop.DiceBase unlock) => save.bases.unlocks[(int)unlock];
	public bool Unlocked(Shop.DiceIcon unlock) => save.icons.unlocks[(int)unlock];
	public bool Unlocked(Shop.DiceInk unlock) => save.inks.unlocks[(int)unlock];

	public void SetBody(Shop.DiceBase body) => save.body = (int)body;
	public void SetIcon(Shop.DiceIcon icon) => save.icon = (int)icon;
	public void SetInk(Shop.DiceInk ink) => save.ink = (int)ink;

	public int GetBody() => save.body;
	public int GetIcon() => save.icon;
	public int GetInk() => save.ink;

	[ContextMenu("Save File")]
	public string Write()
	{
		string jsonRaw = JsonUtility.ToJson(save);
		string encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptDecrypt(jsonRaw)));
		File.WriteAllText(path + Path.DirectorySeparatorChar + "save.dat", encodedData);
		return encodedData;
	}

	[ContextMenu("Read File")]
	public void Read()
	{
		string encodedData;
		try
		{
			encodedData = File.ReadAllText(path + Path.DirectorySeparatorChar + "save.dat");
		}
		catch (FileNotFoundException)
		{
			UnlockBase(Shop.DiceBase.Black);
			UnlockBase(Shop.DiceBase.White);
			UnlockIcon(Shop.DiceIcon.Colors);
			UnlockInk(Shop.DiceInk.Matte);
			encodedData = Write();
		}
		string jsonRaw = EncryptDecrypt(Encoding.UTF8.GetString(Convert.FromBase64String(encodedData)));
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

	[ContextMenu("Motherlode")]
	void AddCoins() => AddCoins(1500);
}

[Serializable]
public class SaveData
{
	public int coins;

	public CollectionSaveData bases = new CollectionSaveData((int)Shop.DiceBase.Count);
	public CollectionSaveData icons = new CollectionSaveData((int)Shop.DiceIcon.Count);
	public CollectionSaveData inks = new CollectionSaveData((int)Shop.DiceInk.Count);

	public int body;
	public int icon;
	public int ink;
}

[Serializable]
public class CollectionSaveData
{
	public bool[] unlocks;

	public CollectionSaveData(int count)
	{
		unlocks = new bool[count];
	}
}
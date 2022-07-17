using System;
using System.Text;
using UnityEngine;
using System.IO;
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
		Debug.Log(path);
		try
		{
			Read();
		}
		catch (FileNotFoundException)
		{
			Write();
			throw;
		}
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

	public void UnlockBase(Shop.DiceBase unlock) => save.baseUnlocks[(int)unlock] = true;
	public void UnlockIcon(Shop.DiceIcon unlock) => save.iconUnlocks[(int)unlock] = true;
	public void UnlockShinyIcons() => save.iconShiny = true;


	[ContextMenu("Save File")]
	public void Write()
	{
		string jsonRaw = JsonUtility.ToJson(save);
		string encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptDecrypt(jsonRaw)));
		File.WriteAllText(path + Path.DirectorySeparatorChar + "save.dat", encodedData);
	}

	[ContextMenu("Read File")]
	public void Read()
	{
		string encodedData = File.ReadAllText(path + Path.DirectorySeparatorChar + "save.dat");
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
}

[Serializable]
public class SaveData
{
	public int coins;
	public bool[] baseUnlocks = new bool[(int)Shop.DiceBase.Count];
	public bool[] iconUnlocks = new bool[(int)Shop.DiceIcon.Count];
	public bool iconShiny;
}
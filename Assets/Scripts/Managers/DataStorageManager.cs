using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorageManager : MonoBehaviour
{
	public enum DataType
	{
		Int,
		Float,
		String,
	}
	public static void SaveData(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}
	
	public static void SaveData(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}
	
	public static void SaveData(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	public static string LoadData(string key)
	{
		return PlayerPrefs.GetInt(key).ToString() != "0" ? PlayerPrefs.GetInt(key).ToString() 
			: PlayerPrefs.GetString(key) != "" ? PlayerPrefs.GetString(key) 
			: PlayerPrefs.GetFloat(key).ToString();
	}
}

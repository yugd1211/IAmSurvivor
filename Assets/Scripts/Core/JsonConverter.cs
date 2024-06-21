using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class JsonConverter
{
	public static void Save<T>(T objectToSave, string fileName)
	{
		fileName = Application.persistentDataPath + fileName;
		CreateNewFile(objectToSave, fileName);
	}

	public static bool Load<T>(out T objectToLoad, string fileName)
	{
		fileName = Application.persistentDataPath + fileName;
		if (File.Exists(fileName))
		{
			string jsonData = File.ReadAllText(fileName);
			if (jsonData != string.Empty)
			{
				objectToLoad = JsonConvert.DeserializeObject<T>(jsonData);
				return true;
			}
		}
		objectToLoad = default;
		return false;
	}	
	
	private static void CreateNewFile<T>(T objectToLoad, string fileName)
	{
		objectToLoad ??= default;
		string jsonData = JsonConvert.SerializeObject(objectToLoad);
		if (!File.Exists(fileName))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName) ?? string.Empty);
			File.Create(fileName).Dispose();
		}
		if (File.Exists(fileName))
			File.WriteAllText(fileName, jsonData);
	}

	public static void DeleteJson(string fileName)
	{ 
		if (File.Exists(fileName))
			File.Delete(fileName);
	}
}
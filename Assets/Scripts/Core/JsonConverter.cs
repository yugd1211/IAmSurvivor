using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public static class JsonConverter
{
	public static void Save<T>(T objectToSave, string fileName)
	{
		fileName = Application.dataPath + fileName;
		CreateNewFile(objectToSave, fileName);
	}

	public static bool Load<T>(out T objectToLoad, string fileName)
	{
		fileName = Application.dataPath + fileName;
		if (File.Exists(fileName))
		{
			using FileStream stream = new FileStream(fileName, FileMode.Open);
			byte[] data = new byte[stream.Length];
			stream.Read(data, 0, (int)stream.Length);
			string jsonData = Encoding.UTF8.GetString(data);
			objectToLoad = JsonConvert.DeserializeObject<T>(jsonData);
			stream.Close();
			return true;
		}
		else
		{
			objectToLoad = default;
			return false;
		}
	}
	private static void CreateNewFile<T>(T objectToLoad, string fileName)
	{
		if (objectToLoad == null)
			objectToLoad = default;
		string jsonData = JsonConvert.SerializeObject(objectToLoad);
		File.WriteAllText(fileName, jsonData);
	}

	public static void DeleteJson(string fileName)
	{ 
		if (File.Exists(fileName))
			File.Delete(fileName);
	}
}
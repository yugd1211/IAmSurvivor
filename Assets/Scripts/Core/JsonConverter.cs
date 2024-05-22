using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class JsonConverter : MonoBehaviour
{
	public static void Save<T>(T objectToSave, string fileName)
	{
		fileName = Application.dataPath + fileName;
		using FileStream stream = new FileStream(fileName, FileMode.Create);
		string jsonData = JsonConvert.SerializeObject(objectToSave);
		byte[] data = Encoding.UTF8.GetBytes(jsonData);
		stream.Write(data, 0, data.Length);
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
			CreateNewFile(objectToLoad, fileName);
			return false;
		}
	}
	private static void CreateNewFile<T>(T objectToLoad, string fileName)
	{
		// 기본값 또는 빈 객체 생성
		// 예를 들어, 만약 T가 List<string>이면, 빈 리스트 생성 가능
		// T defaultObject = default;

		// 객체를 JSON 형식으로 직렬화하여 파일에 쓰기
		string jsonData = JsonConvert.SerializeObject(objectToLoad);
		File.WriteAllText(fileName, jsonData);
	}

	public static void DeleteJson(string fileName)
	{ 
		if (File.Exists(fileName))
			File.Delete(fileName);
	}
}
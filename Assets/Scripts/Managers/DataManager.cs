using System;
using System.Collections.Generic;
using UnityEngine;

public struct PlayLog
{
	public double PlayTime;
	public long KillCount;
	public int VictoryCount;
}

public class DataManager
{
	private static float _creationTime;
	private static PlayLog _playLog;
	private static string _achievePath = "/Json/Achieve.json";
	private static string _playLogPath = "/Json/PlayLog.json";
	private static string _CharacterPath = "/Json/Character.json";

	public static void Init()
	{
		_playLog = LoadPlayLog();
	}

	public static void SaveCharacters(List<int> characters)
	{
		JsonConverter.Save(characters, _CharacterPath);
	}

	public static List<int> LoadCharacters()
	{
		JsonConverter.Load(out List<int> characters, _CharacterPath);
		return characters;
	}

	private static void UpdatePlayTime()
	{
		_playLog.PlayTime = Math.Round(_playLog.PlayTime + (Time.time - _creationTime), 3);
		_creationTime = Time.time;	
	}
	
	public static void SavePlayLog()
	{
		UpdatePlayTime();
		_playLog.KillCount = GameManager.Instance.TotalKill.Count;
		JsonConverter.Save(_playLog, _playLogPath);	
	}
	public static PlayLog LoadPlayLog()
	{
		JsonConverter.Load(out PlayLog playLog, _playLogPath);
		return playLog;
	}
	
	public static void SaveUnlockAchieves(int[] achieveIds)
	{
		JsonConverter.Save(achieveIds, _achievePath);
	}
	
	public static Dictionary<int, Achieve> LoadUnlockAchieves()
	{
		if (!JsonConverter.Load(out int[] unLockAchieves, _achievePath))
			return null;

		Dictionary<int, Achieve> unlockAchieves = new Dictionary<int, Achieve>();
		Dictionary<int, Achieve> achieves = AchieveManager.Instance.Achieves;

		foreach (int id in unLockAchieves)
		{
			if (achieves.TryGetValue(id, out Achieve achieve) && achieve != null)
				unlockAchieves.Add(achieve.id, achieve);
			else
				Debug.Log("달성 업적들 로드 실패");
		}
		return unlockAchieves;
	}

	public static void Victory() => _playLog.VictoryCount++;
}

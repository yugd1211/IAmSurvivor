using System;
using System.Collections.Generic;
using UnityEngine;

public struct PlayLog
{
	public double PlayTime;
	public long KillCount;
	public long HitCount;
	public int VictoryCount;
	public int DefeatCount;
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
		if (JsonConverter.Load(out List<int> characters, _CharacterPath))
			return characters;
		return new List<int>();
	}

	private static void UpdatePlayTime()
	{
		_playLog.PlayTime = Math.Round(_playLog.PlayTime + (Time.time - _creationTime), 3);
		_creationTime = Time.time;	
	}
	
	public static void SavePlayLog()
	{
		StatisticsManager statistics = StatisticsManager.Instance;
		UpdatePlayTime();
		_playLog.KillCount = statistics.GetTotalKillCount();
		_playLog.VictoryCount = statistics.GetVictoryCount();
		_playLog.DefeatCount = statistics.GetDefeatCount();
		_playLog.HitCount = statistics.GetHitCount();
		JsonConverter.Save(_playLog, _playLogPath);	
	}
	public static PlayLog LoadPlayLog()
	{
		if (JsonConverter.Load(out PlayLog playLog, _playLogPath))
			return playLog;
		return new PlayLog();
	}
	
	public static void SaveUnlockAchieves(int[] achieveIds)
	{
		JsonConverter.Save(achieveIds, _achievePath);
	}
	
	public static Dictionary<int, Achieve> LoadUnlockAchieves()
	{
		if (!JsonConverter.Load(out int[] unlockArr, _achievePath))
			return new Dictionary<int, Achieve>();

		Dictionary<int, Achieve> unlockAchieves = new Dictionary<int, Achieve>();
		Dictionary<int, Achieve> achieves = AchieveManager.Instance.Achieves;
		foreach (int id in unlockArr)
		{
			if (achieves.TryGetValue(id, out Achieve achieve) && achieve != null)
				unlockAchieves.Add(achieve.id, achieve);
			else
				Debug.Log("달성 업적들 로드 실패");
		}
		return unlockAchieves;
	}
}

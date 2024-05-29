using Core;
using TMPro;
using UnityEngine;

public class StatisticsManager : Singleton<StatisticsManager>
{
	private long _killCount;
	private long _hitCount;

	private long _totalKillCount;
	private long _totalHitCount;
	private int _victoryCount;
	private int _defeatCount;

	public void InitInGameData()
	{
		_killCount = 0;
		_hitCount = 0;
	}
	
	public void InitPlayLog()
	{
		PlayLog playLog = DataManager.LoadPlayLog();
		
		_totalKillCount = playLog.KillCount;
		_totalHitCount = playLog.HitCount;
		_victoryCount = playLog.VictoryCount;
		_defeatCount = playLog.DefeatCount;
	}

	public void IncrementKillCount() => _killCount++;
	public long GetKillCount() => _killCount;
	public void IncrementTotalKillCount() => _totalKillCount++;
	public long GetTotalKillCount() => _totalKillCount;
	public void IncrementVictoryCount() => _victoryCount++;
	public int GetVictoryCount() => _victoryCount;
	public void IncrementDefeatCount() => _defeatCount++;
	public int GetDefeatCount() => _defeatCount;
	public void IncrementHitCount() => _hitCount++;
	public long GetHitCount() => _hitCount;
	public void IncrementTotalHitCount() => _totalHitCount++;
	public long GetTotalHitCount() => _totalHitCount;
}

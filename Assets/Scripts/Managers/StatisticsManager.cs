using Core;

public class StatisticsManager : Singleton<StatisticsManager>
{
	private int _killCount;
	private long _totalKillCount;
	private int _victoryCount;
	private int _defeatCount;

	public void Init()
	{
		_killCount = 0;
		_totalKillCount = DataManager.LoadPlayLog().KillCount;
	}

	public void IncrementKillCount() => _killCount++;
	public int GetKillCount() => _killCount;
	public void IncrementTotalKillCount() => _totalKillCount++;
	public long GetTotalKillCount() => _totalKillCount;
	public void IncrementVictoryCount() => _victoryCount++;
	public int GetVictoryCount() => _victoryCount;
	public void IncrementDefeatCount() => _defeatCount++;
	public int GetDefeatCount() => _defeatCount;

}

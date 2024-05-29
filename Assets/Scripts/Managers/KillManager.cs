using Core.Observer;

public class KillManager
{ 
	public readonly SubjectKill Kill = new SubjectKill();
	public readonly SubjectKill TotalKill = new SubjectKill();

	public KillManager(int killCount, long totalKillCount)
	{
		Kill.Count = killCount;
		TotalKill.Count = totalKillCount;
	}

	public void IncrementKillCount()
	{
		Kill.Count++;
		TotalKill.Count++;
		StatisticsManager.Instance.IncrementKillCount();
		StatisticsManager.Instance.IncrementTotalKillCount();
	}
}

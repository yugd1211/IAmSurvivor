using Core;
using Core.Observer;

public class KillManager : Singleton<KillManager>
{
	public readonly SubjectKill Kill = new SubjectKill();
	public readonly SubjectKill TotalKill = new SubjectKill();

	public void IncrementKillCount()
	{
		Kill.Count++;
		TotalKill.Count++;
		StatisticsManager.Instance.IncrementKillCount();
		StatisticsManager.Instance.IncrementTotalKillCount();
	}
	
	public void Init()
	{
		Kill.Count = 0;
		TotalKill.Count = (int)DataManager.LoadPlayLog().KillCount;
	}
}

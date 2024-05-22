using System.Linq;
using UnityEngine;

public class AchievementBook : MonoBehaviour
{ 
	public GameObject achievePrefab;
	
	private AchieveManager _achieveManager;
	private AchievePanel[] _achievePanels;

	private void Start()
	{
		_achieveManager = AchieveManager.Instance;
		_achievePanels = new AchievePanel[_achieveManager.UnlockAchieves.Count];
		PullAchieve();
	}

	private void PullAchieve()
	{
		for (int i = 0; i < _achieveManager.UnlockAchieves.Count; i++)
		{
			GameObject instance = Instantiate(achievePrefab, transform);
			_achievePanels[i] = instance.GetComponent<AchievePanel>();
			_achievePanels[i].SetName(_achieveManager.UnlockAchieves.ElementAt(i).Value.name);
			_achievePanels[i].SetDesc(_achieveManager.UnlockAchieves.ElementAt(i).Value.desc);
		}
	}
}

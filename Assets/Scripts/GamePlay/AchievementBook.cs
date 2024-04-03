using UnityEngine;

public class AchievementBook : MonoBehaviour
{ 
	public GameObject achievePrefab;
	
	private AchieveManager _achieveManager = null;
	private AchievePanel[] _achievePanels;

	private void Start()
	{
		_achieveManager = AchieveManager.Instance;
		// todo achieves > lockAchieves
		_achievePanels = new AchievePanel[_achieveManager.Achieves.Count];
		PullAchieve();
	}
	private void OnEnable()
	{
		if (!_achieveManager)
			return;
	}

	private void PullAchieve()
	{
		for (int i = 0; i < _achieveManager.Achieves.Count; i++)
		{
			GameObject instance = Instantiate(achievePrefab, transform);
			_achievePanels[i] = instance.GetComponent<AchievePanel>();
			_achievePanels[i].SetName(_achieveManager.Achieves[i].name);
			_achievePanels[i].SetDesc(_achieveManager.Achieves[i].desc);
		}
	}
}

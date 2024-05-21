using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelcectCharacterList : MonoBehaviour
{
	public GameObject[] lockCharacter;
	public GameObject[] unlockCharacter;
	private readonly AchieveManager.Unlock[] _achieves = (AchieveManager.Unlock[])Enum.GetValues(typeof(AchieveManager.Unlock));

	private void Start()
	{
		UnlockCharacter();
	}

	private void LateUpdate()
	{
		foreach (AchieveManager.Unlock achieve in _achieves)
		{
			CheckUnlockCharacter(achieve);
		}   
	}

	void CheckUnlockCharacter(AchieveManager.Unlock unlock)
	{
		bool isAchieve = false;

		switch (unlock)
		{
			case AchieveManager.Unlock.UnlockPotato:
				// isAchieve = GameManager.Instance.kill >= 10;
				break;
			case AchieveManager.Unlock.UnlockBean:
				isAchieve = GameManager.Instance.gameTime >= GameManager.Instance.maxGameTime;
				break;
		}
	}
	private void UnlockCharacter()
	{
		for (int i = 0; i < lockCharacter.Length; i++)
		{
			string achieveName = _achieves[i].ToString();
			bool isUnlock = int.Parse(DataStorageManager.LoadData(achieveName)) == 1;
			// bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            
			lockCharacter[i].SetActive(!isUnlock);
			unlockCharacter[i].SetActive(isUnlock);
		}
	}
}

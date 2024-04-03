using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpButton : MonoBehaviour
{
	public void NoticeAchieve(int index)
	{
		AchieveManager.Instance.Notice("achieve", index);
	}

	public void NoticeCharacter(int index)
	{
		AchieveManager.Instance.Notice("character", index);
	}
}

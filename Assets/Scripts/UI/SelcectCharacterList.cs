using System.Collections.Generic;
using UnityEngine;

public class SelcectCharacterList : MonoBehaviour
{
	public GameObject[] lockCharacter;
	public GameObject[] unlockCharacter;

	private void Start()
	{
		UnlockCharacter();
	}

	private void UnlockCharacter()
	{
		List<int> characters = DataManager.LoadCharacters();
		if (characters == null)
			return;
		foreach (int item in characters)
		{
			lockCharacter[item].SetActive(false);
			unlockCharacter[item].SetActive(true);
		}
	}
}

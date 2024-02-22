using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;
    
    
    enum Achieve 
    { 
        UnlockPotato,
        UnlockBean, 
    }
    private Achieve[] _achieves;
    private WaitForSecondsRealtime _wait;
    
    private void Awake()
    {
        _achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        _wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("InitAchieve"))
            Init();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
            UnlockCharacter();
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in _achieves)
        {
            CheckAchieve(achieve);
        }   
    }
    void Init()
    {
        PlayerPrefs.SetInt("InitAchieve", 1);
        foreach (Achieve achieve in _achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
        
    }
    
    private void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = _achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1 ? true : false;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.Instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.Instance.gameTime >= GameManager.Instance.maxGameTime;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    private IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
;
        yield return _wait;
        uiNotice.SetActive(false);
    }

}

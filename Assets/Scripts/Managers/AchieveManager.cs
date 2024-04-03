using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public partial class AchieveManager : Singleton<AchieveManager>
{
    [Header("# Inspector Allocate")]
    public NoticeBuilder noticeBuilder;

    [Header("# Play Log")]
    public readonly Dictionary<int, Achieve> Achieves = new Dictionary<int, Achieve>();
    public readonly Dictionary<int, Achieve> UnlockAchieves = new Dictionary<int, Achieve>();
    public int kill;
    public int hit;
    public int hitCount;
    public int victoryCount;
    private Unlock[] _achieves;
    private readonly WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(5);

    public enum Unlock 
    { 
        UnlockPotato,
        UnlockBean, 
    }

    /// todo : 파일에서 파싱 가능하게 수정해야함
    private void InitAchieve()
    {
        Achieve achieve = new Achieve("좀비 학살자", "좀비를 100마리 처치했습니다.");
        // Achieve.Kill newAchieveKill = ;
        achieve.AddCondition(new Achieve.Kill(EnemyType.None, 0, 100));
        // todo : gameManager에 attach하는것도 condition에 Kill이 add되면 알아서 attach하게 변경하는게 좋을듯
        // kill이면 kill에 victory면 victory에....victory같은 경우는 함수니까 좀더 고민해봐야할듯
        // GameManager.Instance.Kill.Attach(newAchieveKill.OKill);
        Achieves.Add(0, achieve);
        achieve = new Achieve("I AM SURVIVOR", "살아남았습니다!!!");
        achieve.AddCondition(new Achieve.Victory(1));
        Achieves.Add(1, achieve);
        achieve = new Achieve("회피 마스터", "한번도 맞지않고 살아남았습니다.");
        achieve.AddCondition(new Achieve.Hit(0));
        achieve.AddCondition(new Achieve.Victory(AchieveManager.Instance.victoryCount + 1));
        Achieves.Add(2, achieve);
        achieve = new Achieve("10킬 ", "10킬"); 
        // newAchieveKill = new Achieve.Kill(EnemyType.None, 0, 10);
        achieve.AddCondition(new Achieve.Kill(EnemyType.None, 0, 10));
        // GameManager.Instance.Kill.Attach(newAchieveKill.OKill);
        Achieves.Add(3, achieve);
    }
    
    protected override void AwakeInit()
    {
        _achieves = (Unlock[])Enum.GetValues(typeof(Unlock));
        if (DataStorageManager.LoadData("InitAchieve") == "0")
            Init();
    }

    private void Start()
    {
        InitAchieve();
    }

    private void Init()
    {
        DataStorageManager.SaveData("InitAchieve", 1);
        foreach (Unlock achieve in _achieves)
        {
            DataStorageManager.SaveData(achieve.ToString(), 0);
        }
    }

    /// <summary>
    /// 테스트용 임시함수
    /// </summary>
    /// <param name="Notice의 타입을 string으로 character, achieve"></param>
    /// <param name="index"></param>
    public void Notice(string type, int index)
    {
        if (type == "character")
        {
            if (index == 0)
                noticeBuilder.BuildNotice(Unlock.UnlockPotato);
            else if (index == 1)
                noticeBuilder.BuildNotice(Unlock.UnlockBean);
        }
        else if (type == "achieve")
        {
            noticeBuilder.BuildNotice(Achieves[index]);
        }
    }
    
    // private void LateUpdate()
    // {
    //     foreach (Unlock achieve in _achieves)
    //     {
    //         CheckUnlockCharacter(achieve);
    //     }   
    // }
    //
    // void CheckUnlockCharacter(Unlock unlock)
    // {
    //     bool isAchieve = false;
    //
    //     switch (unlock)
    //     {
    //         case Unlock.UnlockPotato:
    //             // isAchieve = GameManager.Instance.kill >= 10;
    //             break;
    //         case Unlock.UnlockBean:
    //             isAchieve = GameManager.Instance.gameTime >= GameManager.Instance.maxGameTime;
    //             break;
    //     }
    //
    //     if (isAchieve && PlayerPrefs.GetInt(unlock.ToString()) == 0)
    //     {
    //         PlayerPrefs.SetInt(unlock.ToString(), 1);
    //
    //         for (int i = 0; i < uiNotice.transform.childCount; i++)
    //         {
    //             bool isActive = i == (int)unlock;
    //             uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
    //         }
    //         StartCoroutine(NoticeRoutine());
    //     }
    // }
    //
    // private IEnumerator NoticeRoutine()
    // {
    //     uiNotice.SetActive(true);
    //     AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
    //     yield return _wait;
    //     uiNotice.SetActive(false);
    // }
} 


using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

public partial class AchieveManager : Singleton<AchieveManager>
{
    [Header("# Inspector Allocate")]
    public GameObject noticePrefab;
    public NoticeBuilder noticeBuilder;

    [Header("# Play Log")]
    [Header("# Inspector Allocate")]
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
        Achieve achieve = new Achieve(0, "학살자", "적을 100마리 처치했습니다.");
        // 현재는 에너미의 id나 종류별로 kill수를 따로 체크하지 않기 때문에 enemyType은 아무거나 넣어도 전체적인 kill수 체크한다.
        achieve.AddCondition(new Kill(EnemyType.All, 0, 100, GameManager.Instance.TotalKill));
        // Achieve.Kill newAchieveKill = ;
        // 게임매니저의 kill, totalkill을 결정하고해야댐 achieve.AddCondition(new Kill(EnemyType.None, 0, 100,));
        // todo : gameManager에 attach하는것도 condition에 Kill이 add되면 알아서 attach하게 변경하는게 좋을듯
        // kill이면 kill에 victory면 victory에....victory같은 경우는 함수니까 좀더 고민해봐야할듯
        // GameManager.Instance.Kill.Attach(newAchieveKill.OKill);
        Achieves.Add(0, achieve);
        
        achieve = new Achieve(1, "I AM SURVIVOR", "살아남았습니다!!!");
        achieve.AddCondition(new Victory(1));
        Achieves.Add(1, achieve);
        
        achieve = new Achieve(2, "회피 마스터", "한번도 맞지않고 살아남았습니다.");
        achieve.AddCondition(new Hit(0));
        achieve.AddCondition(new Victory(AchieveManager.Instance.victoryCount + 1));
        Achieves.Add(2, achieve);
        
        achieve = new Achieve(3, "10킬 ", "10킬"); 
        achieve.AddCondition(new Kill(EnemyType.Normal, 0, 10, GameManager.Instance.Kill));
        // newAchieveKill = new Achieve.Kill(EnemyType.None, 0, 10);
        // 게임매니저의 kill, totalkill을 결정하고해야댐 achieve.AddCondition(new Kill(EnemyType.None, 0, 10));
        // GameManager.Instance.Kill.Attach(newAchieveKill.OKill);
        Achieves.Add(3, achieve);
    }

    public Achieve AddAchieveToUnlockList(Achieve achieve)
    {
        if (!Achieves.TryGetValue(achieve.id, out Achieve unlockAchieve))
            return null;
        if (!UnlockAchieves.TryAdd(unlockAchieve.id, unlockAchieve))
            return null;
        // achieve
        return unlockAchieve;
    }

    public void NotifyAchieve(Achieve achieve)
    {
        if (achieve == null)
            return;
        if (!noticeBuilder)
            noticeBuilder = Instantiate(noticePrefab, FindObjectOfType<Canvas>().transform).GetComponent<NoticeBuilder>();
        noticeBuilder.BuildNotice(achieve);
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
        foreach (KeyValuePair<int, Achieve> item in Achieves)
        {
            Achieve achieve = item.Value;
            if (achieve.CheckCondition())
            {
                AddAchieveToUnlockList(achieve);
            }
        }
    }

    private void Init()
    {
        DataStorageManager.SaveData("InitAchieve", 1);
        foreach (Unlock achieve in _achieves)
        {
            DataStorageManager.SaveData(achieve.ToString(), 0);
        }
    }

    private void Update()
    {
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
            // noticeBuilder.BuildNotice(unlockAc[index]);
        }
    }
} 


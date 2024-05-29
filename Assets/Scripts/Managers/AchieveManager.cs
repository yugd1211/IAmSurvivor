using System.Collections.Generic;
using Core;
using UnityEngine;

public enum CharacterType 
{ 
    Potato,
    Bean,
}

public partial class AchieveManager : Singleton<AchieveManager>
{
    [Header("# Inspector Allocate")]
    public GameObject noticePrefab;
    public NoticeBuilder noticeBuilder;

    [Header("# Inspector Allocate")]
    public readonly Dictionary<int, Achieve> Achieves = new Dictionary<int, Achieve>();
    public Dictionary<int, Achieve> UnlockAchieves = null;
    public int kill;
    public int hit;
    public int hitCount;
    public int victoryCount;
    private readonly WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(5);

    /// todo : 파일에서 파싱 가능하게 수정해야함
    private void InitAchieve()
    {
        Achieve achieve = new Achieve(0, "학살자", "적을 100마리 처치했습니다.");
        achieve.AddCondition(new Kill(EnemyType.All, 0, 100, GameManager.Instance.KillManager.TotalKill));
        Achieves.Add(achieve.id, achieve);
        achieve.ConditionsMetAction += () =>
        {
            List<int> charList = DataManager.LoadCharacters();
            charList.Add(0);
            NotifyCharacter(CharacterType.Potato);
            DataManager.SaveCharacters(charList);
            DataManager.SavePlayLog();
        };
        
        achieve = new Achieve(1, "I AM SURVIVOR", "살아남았습니다!!!");
        achieve.AddCondition(new Victory(1));
        Achieves.Add(achieve.id, achieve);
        achieve.ConditionsMetAction += () =>
        {
            List<int> charList = DataManager.LoadCharacters();
            charList.Add(1);
            NotifyCharacter(CharacterType.Bean);
            DataManager.SaveCharacters(charList);
            DataManager.SavePlayLog();
        };
        
        achieve = new Achieve(2, "회피 마스터", "한번도 맞지않고 살아남았습니다.");
        achieve.AddCondition(new Hit(0));
        Achieves.Add(achieve.id, achieve);
        
        achieve = new Achieve(3, "10킬 ", "10킬"); 
        achieve.AddCondition(new Kill(EnemyType.Normal, 0, 10, GameManager.Instance.KillManager.Kill));
        Achieves.Add(achieve.id, achieve);

        UnlockAchieves = DataManager.LoadUnlockAchieves();
    }

    public Achieve AddAchieveToUnlockList(Achieve achieve)
    {
        if (!Achieves.TryGetValue(achieve.id, out Achieve unlockAchieve))
            return null;
        if (!UnlockAchieves.TryAdd(unlockAchieve.id, unlockAchieve))
            return null;
        return unlockAchieve;
    }
    
    // public void NotifyAchieve<T>(Achieve achieve)
    // {
    //     
    // }
    

    public void NotifyAchieve(Achieve achieve)
    {
        if (achieve == null)
            return;
        if (!noticeBuilder)
            noticeBuilder = Instantiate(noticePrefab, FindObjectOfType<Canvas>().transform).GetComponent<NoticeBuilder>();
        noticeBuilder.BuildNotice(achieve);
    }

    public void NotifyCharacter(CharacterType type)
    {
        if (!noticeBuilder)
            noticeBuilder = Instantiate(noticePrefab, FindObjectOfType<Canvas>().transform).GetComponent<NoticeBuilder>();
        noticeBuilder.BuildNotice(type);
    }

    private void Start()
    {
        InitAchieve();
    }
    
}
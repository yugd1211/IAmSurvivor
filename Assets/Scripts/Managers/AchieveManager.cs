using System.Collections.Generic;
using Core;
using UnityEngine;

public enum CharacterType 
{ 
    Potato,
    Bean,
}

public class AchieveManager : Singleton<AchieveManager>
{
    [Header("# Inspector Allocate")]
    public GameObject noticePrefab;
    public NoticeBuilder noticeBuilder;

    [Header("# Inspector Allocate")]
    public readonly Dictionary<int, Achieve> Achieves = new Dictionary<int, Achieve>();
    public Dictionary<int, Achieve> UnlockAchieves;

    private void Start()
    {
        InitAchieve();
    }
    private void InitAchieve()
    {
        Achieve achieve = new Achieve(0, "학살자", "적을 100마리 처치했습니다.");
        achieve.AddCondition(new Kill(EnemyType.All, 0, 100, GameManager.Instance.KillManager.TotalKill));
        Achieves.Add(achieve.id, achieve);
        achieve.ConditionsMetAction += () =>
        {
            List<int> charList = DataManager.LoadCharacters();
            if (charList.Contains((int)CharacterType.Potato))
                return;
            charList.Add(0);
            ProgressNotify(CharacterType.Potato);
            DataManager.SaveCharacters(charList);
        }; 

        
        achieve = new Achieve(1, "I AM SURVIVOR", "살아남았습니다!!!");
        achieve.AddCondition(new Victory(1));
        Achieves.Add(achieve.id, achieve);
        achieve.ConditionsMetAction += () =>
        {
            List<int> charList = DataManager.LoadCharacters();
            if (charList.Contains((int)CharacterType.Bean))
                return;
            charList.Add(1);
            ProgressNotify(CharacterType.Bean);
            DataManager.SaveCharacters(charList);
        };
        
        achieve = new Achieve(2, "회피 마스터", "한번도 맞지않고 살아남았습니다.");
        achieve.AddCondition(new Hit(0));
        Achieves.Add(achieve.id, achieve);
        
        
        achieve = new Achieve(3, "초급 생존자", "적을 10마리 처치했습니다.");
        achieve.AddCondition(new Kill(EnemyType.All, 0, 10, GameManager.Instance.KillManager.TotalKill));
        Achieves.Add(achieve.id, achieve);
        achieve = new Achieve(4, "고급 생존자", "적을 90마리 처치했습니다.");
        achieve.AddCondition(new Kill(EnemyType.All, 0, 90, GameManager.Instance.KillManager.TotalKill));
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
    
    public void ProgressNotify<T>(T notifiedNode)
    {
        if (!noticeBuilder)
            noticeBuilder = Instantiate(noticePrefab, FindObjectOfType<Canvas>().transform).GetComponent<NoticeBuilder>();
        switch (notifiedNode)
        {
            case Achieve achieve:
                noticeBuilder.BuildNotice(achieve);
                break;
            case CharacterType character:
                noticeBuilder.BuildNotice(character);
                break;
        }
    }
}
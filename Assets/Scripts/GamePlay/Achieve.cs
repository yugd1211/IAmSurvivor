using System;
using System.Collections.Generic;
using System.Linq;
using Core.Observer;

// 방문자패턴
[Serializable]
public class Achieve : AObserver
{
    public int id;
    public string name;
    public string desc;
    public List<AchieveCondition> Conditions;
    public Action ConditionsMetAction;
    private ConditionChecker _checker;
    
    public Achieve(int id, string name, string desc)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        _checker = new ConditionChecker();
        Conditions = new List<AchieveCondition>();
        ConditionsMetAction += DataManager.SavePlayLog;
    }

    // 현재 Achieve에 조건을 추가함
    public void AddCondition(AchieveCondition achieveCondition)
    {
        Conditions.Add(achieveCondition);
        achieveCondition.Attach(this);
        // 당연히 Achieve에 추가하고 kill일 경우에는 GameManager의 kill(옵저버 패턴의 subject)에 Attach해서
        // GameManager의 Kill이 변동될때마다 알림받을 수 있게함
        // AchieveCondition에서 attach하고 있음
    }
    
    public void RemoveCondition(AchieveCondition achieveCondition)
    {
        Conditions.Remove(achieveCondition);
    }
    
    public bool CheckCondition()
    {
        for (int i = 0; i < Conditions.Count; i++)
        {
            AchieveCondition item = Conditions[i];
            if (!item.Accept(_checker))
                return false;
        }
        return true;
    }

    public override void Notify(ASubject subject)
    {
        if (!CheckCondition())
            return;
        ConditionsMetAction?.Invoke();
        Achieve achieve = AchieveManager.Instance.AddAchieveToUnlockList(this);
        if (achieve != null)
        {
            AchieveManager.Instance.ProgressNotify(achieve);
            int[] acheiveIds = new int[AchieveManager.Instance.UnlockAchieves.Count];
            for (int i = 0; i < AchieveManager.Instance.UnlockAchieves.Count; i++)
            {
                acheiveIds[i] = AchieveManager.Instance.UnlockAchieves.ElementAt(i).Key;
            }
            DataManager.SaveUnlockAchieves(acheiveIds);
        }
    }
}

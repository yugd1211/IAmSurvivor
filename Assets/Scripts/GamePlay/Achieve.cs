using System;
using System.Collections.Generic;
using System.Linq;
using Core.Observer;

[Serializable]
public class Achieve : IObserver
{
    public int id;
    public string name;
    public string desc; 
    public Action ConditionsMetAction;
    
    private List<AchieveCondition> _conditions;
    private ConditionFulfillmentChecker _fulfillmentChecker;
    
    public Achieve(int id, string name, string desc)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        _fulfillmentChecker = new ConditionFulfillmentChecker();
        _conditions = new List<AchieveCondition>();
        ConditionsMetAction += DataManager.SavePlayLog;
    }

    public void AddCondition(AchieveCondition achieveCondition)
    {
        _conditions.Add(achieveCondition);
        achieveCondition.Attach(this);
    }
    
    public void RemoveCondition(AchieveCondition achieveCondition)
    {
        _conditions.Remove(achieveCondition);
    }
    
    public bool CheckCondition()
    {
        for (int i = 0; i < _conditions.Count; i++)
        {
            AchieveCondition item = _conditions[i];
            if (!item.Accept(_fulfillmentChecker))
                return false;
        }
        return true;
    }

    public void Notify(ISubject subject)
    {
        AchieveManager achieveManager = AchieveManager.Instance;
        if (!CheckCondition())
            return;
        ConditionsMetAction?.Invoke();
        Achieve achieve = achieveManager.AddAchieveToUnlockList(this);
        if (achieve != null)
        {
            achieveManager.ProgressNotify(achieve);
            int[] acheiveIds = new int[achieveManager.UnlockAchieves.Count];
            for (int i = 0; i < achieveManager.UnlockAchieves.Count; i++)
            {
                acheiveIds[i] = achieveManager.UnlockAchieves.ElementAt(i).Key;
            }
            DataManager.SaveUnlockAchieves(acheiveIds);
        }
    }
}

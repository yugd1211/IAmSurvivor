using Core.Observer;

public abstract class AchieveCondition : ASubject
{
    public abstract bool Accept(IConditionVisitor visitor);
}

public interface IConditionVisitor
{
    bool Visit(Kill kill);
    bool Visit(Hit hit);
    bool Visit(HP hp);
    bool Visit(Victory victory);
}

public class ConditionFulfillmentChecker : IConditionVisitor
{
    public bool Visit(Kill kill)
    {
        return kill.SKill.Count >= kill.OKill.GoalCount;
    }
    public bool Visit(Hit hit)
    {
        return hit.IsVictory && StatisticsManager.Instance.GetHitCount() <= hit.HitCount;
    }
    public bool Visit(HP hp)
    {
        return hp.IsVictory && GameManager.Instance.player.health / GameManager.Instance.player.maxHealth * 100 >= hp.HpPercent;
    }
    public bool Visit(Victory victory)
    {
        return StatisticsManager.Instance.GetVictoryCount() >= victory.VictoryCount;
    }
}

public class Kill : AchieveCondition
{
    public readonly EnemyType EnemyType;
    public readonly int ID;
    public readonly ObserverKill OKill;
    public readonly SubjectKill SKill;

    public Kill(EnemyType type, int id, int killCount, SubjectKill attachKill)
    {
        EnemyType = type;
        ID = id;
        SKill = attachKill;
        OKill = new ObserverKill(killCount);
        if (SKill.Count >= OKill.GoalCount)
            return;
        attachKill.Attach(OKill);
        OKill.Action += () =>
        {
            NotifyObservers();
            attachKill.Detach(OKill);
        };
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class Hit : AchieveCondition 
{
    public readonly int HitCount;
    public bool IsVictory;
    public Hit(int hitCount)
    {
        IsVictory = false;
        HitCount = hitCount;
        GameManager.Instance.VictoryAction += () =>
        {
            IsVictory = true;
            NotifyObservers(); 
        };
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class HP : AchieveCondition
{
    public readonly int HpPercent;
    public bool IsVictory;

    public HP(int hpPercent)
    {
        HpPercent = hpPercent;
        IsVictory = false;
        GameManager.Instance.VictoryAction += () =>
        {
            IsVictory = true;
            NotifyObservers(); 
        };
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class Victory : AchieveCondition 
{
    public readonly int VictoryCount;
    public Victory(int victoryCount)
    {
        VictoryCount = victoryCount;
        GameManager.Instance.VictoryAction += NotifyObservers;
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

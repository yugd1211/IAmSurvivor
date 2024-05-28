using Core.Observer;
using UnityEngine;

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

// todo : 킬 감지를 어떤식으로 할지 더 확실하게 해야할듯
public class ConditionChecker : IConditionVisitor
{
    public bool Visit(Kill kill)
    {
        return kill.SKill.Count >= kill.OKill.GoalCount;
    }
    public bool Visit(Hit hit)
    {
        Debug.Log("Hit Visit");
        return AchieveManager.Instance.hitCount >= hit.HitCount;
    }
    public bool Visit(HP hp)
    {
        return GameManager.Instance.health / GameManager.Instance.maxHealth * 100 >= hp.HpPercent;
    }
    public bool Visit(Victory victory)
    {
        return AchieveManager.Instance.victoryCount >= victory.VictoryCount;
    }
}

// todo : GameManager.totalKill을 subject로 바꿀지 아니면 Observer인 상태로 둘지를 정해야할듯
// 그리고 서브젝트에 등록할텐데 해당 subject로부터 detach를 해야하니 totalKill도 subject인게 좋을듯..
// 
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
            // todo : 문제가 있음, 언록된 도감도 일단은 attach하기 때문에, 이미 달성된 도감의 조건들도 일단 게임도중 조건이 달성되면 Action을 실행시킴
            // 결론적으로는 이미 unlock됐기 때문에 아무일도 발생하지 않지만, 이미 달성된 도감임에도 불구하고 게임도중에 계속 조건을 확인함...
            // 이것도 사실 처음부터 lock, unlock achieve를 나눠두면 상관없을거같긴함 unlock이면 애초에 attach를...........
            // 생각해보니 애초에 attach를 안하면된다. attach 하기전에 이미 달성된 조건인지 아닌지를 먼저 판단해버리자
            // 다시 생각해보니까 lock, unlock은 따로 구분을 해야됨, 저장되는 데이터 뿐 아니라 특정판을 잘했을경우의 업적도 있기 때문에
            // 해당 업적들은 이미 달성됐기 때문에 이를 알고 있어야함
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
    public Hit(int hitCount)
    {
        HitCount = hitCount;
        
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class HP : AchieveCondition
{
    public readonly int HpPercent;
    public HP(int hpPercent)
    {
        HpPercent = hpPercent;
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
    }

    public override bool Accept(IConditionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}


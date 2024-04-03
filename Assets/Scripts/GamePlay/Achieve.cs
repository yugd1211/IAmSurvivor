using System;
using System.Collections.Generic;
using Core.Observer;
using UnityEngine;

// 방문자패턴
[Serializable]
public class Achieve : AObserver
{
    public string name;
    public string desc;
    public List<Condition> Conditions;
    private ConditionChecker _checker;
    
    public Achieve(string name, string desc)
    {
        _checker = new ConditionChecker();
        Conditions = new List<Condition>();
        this.name = name;
        this.desc = desc;
    }

    public void AddCondition(Condition condition)
    {
        Conditions.Add(condition);
        if (condition is Kill kill)
        {
            GameManager.Instance.Kill.Attach(kill.OKill);
            Debug.Log("attach kill");
        }
    }

    public interface IConditionVisitor
    {
        bool Visit(Kill kill);
        bool Visit(Hit hit);
        bool Visit(Hp hp);
        bool Visit(Victory victory);
    }

    public abstract class Condition
    {
        public abstract bool Accept(IConditionVisitor visitor);
    }
    
    public class Kill : Condition
    {
        public readonly EnemyType EnemyType;
        public readonly int ID;
        public readonly ObserverKill OKill;        

        public Kill(EnemyType type, int id, int killCount)
        {
            EnemyType = type;
            ID = id;
            OKill = new ObserverKill(killCount);
            OKill.Action += Goal;
        }

        private void Goal()
        {
            Debug.Log("Goal!!");
            GameManager.Instance.Kill.Detach(OKill);
        }
        
        public override bool Accept(IConditionVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
    
    public class Hit : Condition 
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
    
    public class Hp : Condition
    {
        public readonly int HpPercent;
        public Hp(int hpPercent)
        {
            HpPercent = hpPercent;
        }

        public override bool Accept(IConditionVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
    
    public class Victory : Condition 
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

    public class ConditionChecker : IConditionVisitor
    {
        public bool Visit(Kill kill)
        {
            return AchieveManager.Instance.kill == kill.OKill.Count;
        }
        public bool Visit(Hit hit)
        {
            return AchieveManager.Instance.hitCount >= hit.HitCount;
        }
        public bool Visit(Hp hp)
        {
            return GameManager.Instance.health / GameManager.Instance.maxHealth * 100 >= hp.HpPercent;
        }
        public bool Visit(Victory victory)
        {
            return AchieveManager.Instance.victoryCount >= victory.VictoryCount;
        }
    }
    
    private bool CheckCondition()
    {
        foreach (Condition item in Conditions)
        {
            if (!item.Accept(_checker))
                return false;
        }
        return true;
    }

    public override void Notify(ASubject subject)
    {
        CheckCondition();
    }
}

using System;
using UnityEngine;

namespace Core.Observer
{
    public class SubjectKill : ASubject
    {
        private int _count = 0;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyObservers();
            }
        }
    }
    
    public class ObserverKill : AObserver
    {
        public ObserverKill(int goalCount)
        {
            GoalCount = goalCount;
        }
        
        public int Count { get; set; }
        public int GoalCount;
        public Action Action;
        
        public override void Notify(ASubject subject)
        {
            if (subject is SubjectKill kill)
            {
                Count = kill.Count;
                GoalCompleted();
            }
        }

        private void GoalCompleted()
        {
            if (Count < GoalCount)
                return;
            Action();
        } 
    }
}

using System;

namespace Core.Observer
{
    public class SubjectKill : ASubject
    {
        private long _count = 0;
        public long Count
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
        
        public long Count { get; set; }
        public long GoalCount;
        public Action Action;
        
        // subject에게 알림을 받을때 사용하는 함수
        public override void Notify(ASubject subject)
        {
            if (subject is SubjectKill kill)
            {
                Count = kill.Count;
                if (Count >= GoalCount)
                    Action();
            }
        }
    }
}

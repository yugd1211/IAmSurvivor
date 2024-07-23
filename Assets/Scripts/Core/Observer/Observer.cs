using System;

namespace Core.Observer
{
    public interface IObserver
    {
        public void Notify(ISubject subject);
    }
    
    public class ObserverKill : IObserver
    {
        public ObserverKill(int goalCount)
        {
            GoalCount = goalCount;
        }
        
        public long Count { get; set; }
        public long GoalCount;
        public Action Action;
        
        // subject에게 알림을 받을때 사용하는 함수
        public void Notify(ISubject subject)
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

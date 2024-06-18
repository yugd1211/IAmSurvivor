using System.Collections.Generic;

namespace Core.Observer
{
    public abstract class AObserver
    {
        public abstract void Notify(ASubject subject);
    }
    
    public abstract class ASubject
    {
        private readonly List<AObserver> _observers = new List<AObserver>();

        public void Attach(AObserver observer)
        {
            _observers.Add(observer);
        }
        
        public void Detach(AObserver observer)
        { 
            _observers.Remove(observer);
        }

        protected void NotifyObservers()
        {
            // Notify중 Detach했을때 오류가 발생하기 때문에 이전값이 모두 저장된 list를 복사후 진행
            List<AObserver> observersSnapshot = new List<AObserver>(_observers);
            foreach (AObserver observer in observersSnapshot)
            {
                observer.Notify(this);
            }
        }
    }
}

using System.Collections.Generic;

namespace Core.Observer
{
    public interface ISubject
    {
        public void Attach(IObserver observer);
        public void Detach(IObserver observer);
        public void NotifyObservers();
    }

    public class Subject : ISubject
    {      
        private readonly List<IObserver> _observers = new List<IObserver>();
        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void NotifyObservers()
        {
            // Notify중 Detach했을때 오류가 발생하기 때문에 이전값이 모두 저장된 list를 복사후 진행
            List<IObserver> observersSnapshot = new List<IObserver>(_observers);
            foreach (IObserver observer in observersSnapshot)
            {
                observer.Notify(this);
            }
        }
    }
    public class SubjectKill : Subject
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
}
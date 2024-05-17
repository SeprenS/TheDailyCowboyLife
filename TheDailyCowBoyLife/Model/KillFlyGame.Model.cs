using System;
using System.Drawing;
using KillFlyGame.Observer;
using System.Collections.Generic;
using KillFlyGame.Observable;

namespace KillFlyGame.Model
{
    public class FlyModel : IObservable
    {
        private List<IObserver> observers;

        public FlyModel()
        {
            observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer) => observers.Add(observer);

        public void RemoveObserver(IObserver observer) => observers.Remove(observer);

        public void NotifyObservers()
        {
            foreach (var observer in observers)
                observer.Update(this);
        }

        public Point Center { get; set; }
        public int Size { get; set; }
        public bool IsGrowing { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public int Score { get; set; }
        public int MaxSize = 50;
        public int MinSize = 1;
        public Font GameFont { get; set; }
        public int Misses { get; set; }
    }
}

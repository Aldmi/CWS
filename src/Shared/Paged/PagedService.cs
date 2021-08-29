using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Timers;
using Shared.Extensions;

namespace Shared.Paged
{
    public class PagedService<T> : IDisposable
    {
        private readonly Timer _timerInvoke;
        private readonly Subject<Memory<T>> _nextPageSubj = new Subject<Memory<T>>();
        private int _currentPosition;
        private Memory<T> _bufer;

        
        public PagedService(PagedOption option)
        {
            Option = option;
            _timerInvoke = new Timer(Option.Time);
            _timerInvoke.Elapsed += (sender, args) =>
            {
                var nextPage = CalculateNextPage();
                _nextPageSubj.OnNext(nextPage);
            };
        }

        
        public PagedOption Option { get; }
        public IObservable<Memory<T>> NextPageRx => _nextPageSubj.AsObservable();


        
        public void SetData(Memory<T> data)
        {
            var compareRes = data.Compare(_bufer);
            if (compareRes.IsFailure)
            {
                _bufer = data;
                Reset();
                var nextPage = CalculateNextPage();
                _nextPageSubj.OnNext(nextPage);
            }
        }


        private Memory<T> CalculateNextPage()
        {
            if (_bufer.IsEmpty)
                return _bufer;
            
            var count = Option.Count;
            if (_currentPosition + Option.Count >= _bufer.Length)
            {
                count = _bufer.Length - _currentPosition;
            }
            var nextPageMem= _bufer.Slice(_currentPosition, count);
            _currentPosition += nextPageMem.Length;
            if (_currentPosition == _bufer.Length)
            {
                _currentPosition = 0;
            }
            return nextPageMem;
        }
        
        
        private void Reset()
        {
            _currentPosition = 0;
            _timerInvoke.Stop();
            _timerInvoke.Interval = Option.Time;
            _timerInvoke.Start();
        }
        
        
        public void Dispose()
        {
            _timerInvoke?.Dispose();
        }
    }
}
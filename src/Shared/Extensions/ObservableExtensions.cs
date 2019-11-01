using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class MyObservableExtensions
    {
        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<Task> onNextAsync) =>
            source
                .Select(number => Observable.FromAsync(onNextAsync))
                .Concat()
                .Subscribe();

        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync) =>
            source
                .Select(number => Observable.FromAsync(async () => await onNextAsync(number)))
                .Concat()
                .Subscribe();

        public static IDisposable SubscribeAsyncConcurrent<T>(this IObservable<T> source, Func<Task> onNextAsync) =>
            source
                .Select(number => Observable.FromAsync(onNextAsync))
                .Merge()
                .Subscribe();

        public static IDisposable SubscribeAsyncConcurrent<T>(this IObservable<T> source, Func<T,Task> onNextAsync) =>
            source
                .Select(number => Observable.FromAsync(async () => await onNextAsync(number)))
                .Merge()
                .Subscribe();


        public static IDisposable SubscribeAsyncConcurrent<T>(this IObservable<T> source, Func<Task> onNextAsync, int maxConcurrent) =>
            source
                .Select(number => Observable.FromAsync(onNextAsync))
                .Merge(maxConcurrent)
                .Subscribe();
    }
}
using System.Collections.Concurrent;
using System.Linq;
using CSharpFunctionalExtensions;
using KellermanSoftware.CompareNetObjects;

namespace Shared.Collections
{
    /// <summary>
    /// Потокобезопасная очередь уникальных элементов (без дупликатов) с ограничением по размеру.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitConcurrentQueueWithoutDuplicate<T>
    {
        #region field

        private readonly bool _extractLastItem;
        private readonly int _limitItems;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        #endregion



        #region prop

        public int Count => _queue.Count;
        public bool IsEmpty => _queue.IsEmpty;
        public bool IsOneItem => (_queue.Count == 1);
        public  bool IsFullLimit => (_queue.Count >= _limitItems);

        #endregion



        #region ctor

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="extractLastItem">Извлекать или нет последний элемент из очереди. Если не извлекать то по команде Dequeue всегда показывается последний элемент</param>
        /// <param name="limitItems">Ограничение по размеру очереди</param>
        public LimitConcurrentQueueWithoutDuplicate(bool extractLastItem, int limitItems)
        {
            _extractLastItem = extractLastItem;
            _limitItems = limitItems;
        }

        #endregion



        #region Methode

        /// <summary>
        /// Добавить элемент в конец очереди.
        /// Если элементов больше _limitItems, элемент не добавляется.
        /// Если элемент уже присутсвует в очереди, элемент не добавляется.
        /// </summary>
        /// <param name="item">элемент для добавления в конец очереди</param>
        /// <returns></returns>
        public Result<T> Enqueue(T item)
        {
            if (IsFullLimit)
                return Result.Fail<T>("Max Limit fail");

            if (Contains(item))
                return Result.Fail<T>("Element already exist");

            _queue.Enqueue(item);
            return Result.Ok(item);
        }


        /// <summary>
        /// Извлечь элемент из головы очереди.
        /// Если очередь пуста - вернуть Fail
        /// Если возникнут concurrent ошибки при извлечении - вернуть Fail
        /// </summary>
        /// <returns></returns>
        public Result<T, DequeueResultErrorWrapper> Dequeue()
        {
            if (IsEmpty)
            {
                return Result.Fail<T, DequeueResultErrorWrapper>(new DequeueResultErrorWrapper(DequeueResultError.Empty));
            }
            T res;
            if (IsOneItem)
            {
                if (_extractLastItem)
                {
                    return _queue.TryDequeue(out res) ?
                        Result.Ok<T, DequeueResultErrorWrapper>(res) :
                        Result.Fail<T, DequeueResultErrorWrapper>(new DequeueResultErrorWrapper(DequeueResultError.FailTryDequeue));
                }
                return _queue.TryPeek(out res) ?
                    Result.Ok<T, DequeueResultErrorWrapper>(res) :
                    Result.Fail<T, DequeueResultErrorWrapper>(new DequeueResultErrorWrapper(DequeueResultError.FailTryPeek));
            }
            return _queue.TryDequeue(out res) 
                ? Result.Ok<T, DequeueResultErrorWrapper>(res) 
                : Result.Fail<T, DequeueResultErrorWrapper>(new DequeueResultErrorWrapper(DequeueResultError.FailTryDequeue));
        }


        /// <summary>
        /// Содержит ли очередь такой эленмент
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool Contains(T item)
        {
            var findItem = _queue.FirstOrDefault(i => Comparer(i, item).IsSuccess);
            return findItem != null;
        }


        /// <summary>
        /// Сравнить 2 элемента.
        /// </summary>
        private Result<bool> Comparer(T obj1, T obj2)
        {
            var compareLogic = new CompareLogic { Config = { MaxMillisecondsDateDifference = 1000 } };
            ComparisonResult result = compareLogic.Compare(obj1, obj2);
            return result.AreEqual ? Result.Ok(true) : Result.Fail<bool>(result.DifferencesString);
        }

        #endregion
    }





    public class DequeueResultErrorWrapper
    {
        public readonly DequeueResultError DequeueResultError;

        public DequeueResultErrorWrapper(DequeueResultError dequeueResultError)
        {
            DequeueResultError = dequeueResultError;
        }
    }


    public enum DequeueResultError
    {
        Empty,
        FailTryPeek,
        FailTryDequeue
    }
}

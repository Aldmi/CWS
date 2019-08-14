using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using CSharpFunctionalExtensions;
using KellermanSoftware.CompareNetObjects;

namespace Shared.Collections
{
    /// <summary>
    /// Потокобезопасная очередь уникальных элементов (без дубликатов) с ограничением по размеру.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitConcurrentQueueWithoutDuplicate<T>
    {
        #region field

        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly QueueMode _queueMode;
        private readonly int _limitItems;
        private T _oneItem;

        #endregion



        #region prop

        public int Count
        {
            get
            {
                if (_queueMode == QueueMode.OneItem)
                {
                    return (_oneItem == null) ? 0 : 1;
                }
                return _queue.Count;
            }
        }


        public bool IsEmpty => (Count == 0 );
        public bool IsOneItem => (Count == 1);
        public bool IsFullLimit => (Count >= _limitItems);

        #endregion



        #region ctor

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="queueMode">Один элемент, всегда перезаписывается / Извлекать последний элемент из очереди / Не извлекать последний элемент из очереди. Если не извлекать то по команде Dequeue всегда показывается последний элемент</param>
        /// <param name="limitItems">Ограничение по размеру очереди</param>
        public LimitConcurrentQueueWithoutDuplicate(QueueMode queueMode, int limitItems)
        {
            _queueMode = queueMode;
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
            switch (_queueMode)
            {
                case QueueMode.OneItem:
                    var cmpRes = Comparer(_oneItem, item);
                    if (cmpRes.IsFailure)
                    {
                        _oneItem = item;
                        return Result.Ok(item);
                    }
                    return Result.Fail<T>("Element already exist");

                case QueueMode.QueueExtractLastItem:
                case QueueMode.QueueNotExtractLastItem:
                    if (IsFullLimit)
                        return Result.Fail<T>("Max Limit fail");

                    if (Contains(item))
                        return Result.Fail<T>("Element already exist");

                    _queue.Enqueue(item);
                    return Result.Ok(item);

                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            if (_queueMode == QueueMode.OneItem)
            {
                return Result.Ok<T, DequeueResultErrorWrapper>(_oneItem);
            }

            T res;
            if (IsOneItem)
            {
                if (_queueMode == QueueMode.QueueExtractLastItem)
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


    /// <summary>
    /// Опции Очереди
    /// </summary>
    public enum QueueMode
    {
        /// <summary>
        /// ConcurrentQueue не используется. Значение хранится в одной переменной.
        /// Enqueue - перезаписывает значение
        /// Dequeue - извлекает текушее значение
        /// </summary>
        OneItem,

        /// <summary>
        /// Значения хранятся в ConcurrentQueue.
        /// Enqueue - добавлет в конец очереди элемент
        /// Dequeue - извлекает элементы до конца очереди, пока не опустошит ее.
        /// </summary>
        QueueExtractLastItem,

        /// <summary>
        /// Значения хранятся в ConcurrentQueue.
        /// Enqueue - добавлет в конец очереди элемент
        /// Dequeue - извлекает элементы пока не останется 1 элемент. Если остался 1 элемент он возвращается при вызове Dequeue и никогда не удаляется
        /// </summary>
        QueueNotExtractLastItem,
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

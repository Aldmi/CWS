﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shared.Enums;

namespace Infrastructure.Storages
{
    public class BaseStorage<TKey, TValue> where TKey : IEquatable<TKey>
                                                  where TValue : class, IDisposable
    {
        #region prop

        private ConcurrentDictionary<TKey, TValue> Storage { get; } = new ConcurrentDictionary<TKey, TValue>();
        public IEnumerable<TValue> Values => Storage.Values;

        #endregion



        #region Methode

        public DictionaryCrudResult AddNew(TKey key, TValue value)
        {
            if (Storage.ContainsKey(key))
            {
                return DictionaryCrudResult.KeyAlredyExist;
            }

            return (Storage.TryAdd(key, value)) ? DictionaryCrudResult.Added : DictionaryCrudResult.None;
        }


        public DictionaryCrudResult Update(TKey key, TValue value)
        {
            if (!Storage.ContainsKey(key))
            {
                return DictionaryCrudResult.KeyNotExist;
            }
            
            return (Storage.TryUpdate(key, value, Storage[key])) ? DictionaryCrudResult.Updated : DictionaryCrudResult.None;
        }


        public DictionaryCrudResult Remove(TKey key)
        {
            if (!Storage.ContainsKey(key))
            {
                return DictionaryCrudResult.KeyNotExist;
            }
            var value = Storage[key];
            value.Dispose();
            return (Storage.TryRemove(key, out value)) ? DictionaryCrudResult.Removed : DictionaryCrudResult.None; 
        }


        public TValue Get(TKey key)
        {
            if (!Storage.ContainsKey(key))
            {
                return null;
            }
            return Storage.TryGetValue(key, out var res) ? res : null;
        }


        public IEnumerable<TValue> GetMany(IEnumerable<TKey> keys)
        {
            return Storage.Where(item => keys.FirstOrDefault(key => key.Equals(item.Key)) != null).Select(pair => pair.Value);
        }


        public bool IsExist(TKey key)
        {
           return Storage.ContainsKey(key);
        }


        public DictionaryCrudResult EraseAll()
        {
            foreach (var s in Storage)
            {
                Remove(s.Key);
            }
            return DictionaryCrudResult.Removed;
        }


        public IReadOnlyDictionary<TKey, TValue> Cast2ReadOnlyDictionary()
        {
            return new ReadOnlyDictionary<TKey, TValue>(Storage);
        }

        #endregion
    }
}
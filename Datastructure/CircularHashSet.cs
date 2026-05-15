using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Datastructure
{
    public class CircularHashSet<T>
    {
        private readonly int _fixedSize;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public CircularHashSet(int fixedSize)
        {
            if (fixedSize <= 0)
                throw new ArgumentException("Fixed size must be greater than 0", nameof(fixedSize));

            _fixedSize = fixedSize;
            LinkedDictionary = new Dictionary<T, (DateTime CreateTime, T? Value)>(_fixedSize);
        }

        private T? OldKey = default;
        private T? PreviousKey = default;
        private Dictionary<T, (DateTime CreateTime, T? Value)> LinkedDictionary;

        public bool TryAdd(T key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _lock.EnterWriteLock();
            try
            {
                if (LinkedDictionary.Count < _fixedSize)
                {
                    return TryAddInternal(key);
                }

                if (OldKey == null)
                    return false;

                var nextOldKey = LinkedDictionary[OldKey];
                LinkedDictionary.Remove(OldKey);
                OldKey = nextOldKey.Value;
                return TryAddInternal(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        

        public T[] GetKeys()
        {
            _lock.EnterReadLock();
            try
            {
                return LinkedDictionary.Keys.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public T[] GetKeysByCreatedTime()
        {
            _lock.EnterReadLock();
            try
            {
                return LinkedDictionary.OrderBy(x=>x.Value.CreateTime).Select(x=>x.Key).ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return LinkedDictionary.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                LinkedDictionary.Clear();
                OldKey = default;
                PreviousKey = default;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool Contains(T key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _lock.EnterReadLock();
            try
            {
                return LinkedDictionary.ContainsKey(key);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private bool TryAddInternal(T key)
        {
            // This method is called within a write lock, so no additional locking needed

            if (LinkedDictionary.ContainsKey(key))
                return false;

            LinkedDictionary.Add(key, (DateTime.Now, default));

            if (LinkedDictionary.Count == 1)
            {
                OldKey = key;
            }
            else
            {
                if (PreviousKey == null)
                    PreviousKey = OldKey;

                LinkedDictionary[PreviousKey!] = (LinkedDictionary[PreviousKey!].CreateTime, key);
                PreviousKey = key;
            }
            return true;
        }

        public void Dispose()
        {
            _lock?.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{

    internal class LruCache<TKey, TValue> where TValue : class
    {
        private readonly Dictionary<TKey, TValue> _cachedDictionary = new Dictionary<TKey, TValue>();
        private readonly List<TKey> _cachesList = new List<TKey>();
        private readonly uint _maxSize;
        private readonly object _thisLock = new object();

        public LruCache(uint maxSize)
        {
            _maxSize = maxSize;
        }

        public void AddObject(TKey key, TValue cacheObject)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            lock (_thisLock)
            {
                if (_cachedDictionary.ContainsKey(key))
                {
                    _cachedDictionary[key] = cacheObject;
                }
                else
                {
                    if (_cachedDictionary.Count + 1 > _maxSize)
                    {
                        _cachedDictionary.Remove(_cachesList.Last());
                        _cachesList.RemoveAt(_cachesList.Count - 1);
                    }
                    _cachedDictionary.Add(key, cacheObject);
                    _cachesList.Insert(0, key);
                }
            }
        }

        public bool Contains(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return _cachedDictionary.ContainsKey(key);
        }

        public TValue GetObject(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            TValue data = null;
            lock (_thisLock)
            {
                if (_cachedDictionary.ContainsKey(key))
                {
                    data = _cachedDictionary[key];
                    _cachesList.Remove(key);
                    _cachesList.Insert(0, key);
                }
            }
            return data;
        }

        public void Clear(Func<TKey, TValue, bool> filter)
        {
            lock (_thisLock)
            {
                for (int i = _cachesList.Count - 1; i >= 0; i--)
                {
                    TKey key = _cachesList[i];
                    if (filter(key, _cachedDictionary[key]))
                    {
                        _cachedDictionary.Remove(key);
                        _cachesList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 清除所有的缓存。
        /// </summary>
        public void ClearAll()
        {
            lock (_thisLock)
            {
                _cachedDictionary.Clear();
                _cachesList.Clear();
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace loconotes.Services
{
    public interface ICacheProvider<T>
    {
        T Set(T value);
        T Get();
        void Clear();
    }

    public abstract class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;

        protected CacheProvider(
            IMemoryCache memoryCache,
            string cacheKey
        )
        {
            _cache = memoryCache;
            _cacheKey = cacheKey;
        }

        public T Set(T value)
        {
            return _cache.Set(_cacheKey, value);
        }

        public T Get()
        {
            T value;
            return _cache.TryGetValue(_cacheKey, out value) ? value : default(T);
        }

        public void Clear()
        {
            _cache.Remove(_cacheKey);
        }
    }
}

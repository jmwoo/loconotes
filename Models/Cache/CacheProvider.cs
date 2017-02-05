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
        private readonly TimeSpan _absoluteExpirationRelativeToNow;

        protected CacheProvider(
            IMemoryCache memoryCache,
            string cacheKey,
            TimeSpan absoluteExpirationRelativeToNow
        )
        {
            _cache = memoryCache;
            _cacheKey = cacheKey;
            _absoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        }

        public T Set(T value)
        {
            return _cache.Set(_cacheKey, value, _absoluteExpirationRelativeToNow);
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

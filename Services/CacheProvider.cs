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
        T Get(string key);
        T Set(string key, T value);
    }

    public class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly IMemoryCache _cache;

        public CacheProvider(
            IMemoryCache memoryCache
        )
        {
            _cache = memoryCache;
        }

        public T Get(string key)
        {
            T value;
            return _cache.TryGetValue(key, out value) ? value : default(T);
        }

        public T Set(string key, T value)
        {
            return _cache.Set(key, value);
        }
    }
}

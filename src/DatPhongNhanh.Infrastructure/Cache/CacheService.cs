using DatPhongNhanh.Application.Common.Cache;
using DatPhongNhanh.Application.User.Queries;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatPhongNhanh.Infrastructure.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            var value = _memoryCache.Get<T>(key);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiration;

            var value2 = _memoryCache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefixKey)
        {
            // Note: This is a limitation of IMemoryCache, 
            // we can't easily remove by prefix without tracking the keys
            return Task.CompletedTask;
        }
    }

    public class DefaultCachePolicy : ICachePolicy
    {

        private readonly Dictionary<Type, TimeSpan> _expirationMap;

        public DefaultCachePolicy()
        {
            _expirationMap = new Dictionary<Type, TimeSpan>()
            {
                { typeof(TestCacheQuery), TimeSpan.FromSeconds(5) }
            };
        }
        public string GenerateKey<T>(params object[] keyParameters)
        {
            var type = typeof(T);
            var key = $"{type.Name}";

            if (keyParameters != null && keyParameters.Length > 0)
            {
                key += ":" + string.Join(":", keyParameters);
            }

            return key;
        }

        public TimeSpan? GetExpirationTime<T>()
        {
            return _expirationMap.TryGetValue(typeof(T), out var time) ? time : null;
        }
    }
}

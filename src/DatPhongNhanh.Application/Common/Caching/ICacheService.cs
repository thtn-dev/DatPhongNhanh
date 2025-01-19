using EasyCaching.Core;

namespace DatPhongNhanh.Application.Common.Caching
{
    public interface ICacheService<T> where T : class
    {
        Task<T?> GetAsync(string key);
        Task SetAsync(string key, T value, TimeSpan? duration = null);
        Task RemoveAsync(string key, List<string>? refKeys);
        Task<T?> GetByRefKey(string key, string refKey);
        Task<T?> GetOrSetAsync (string key, Func<Task<T>> acquire, TimeSpan? duration = null);
    }


    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly IEasyCachingProviderFactory easyCachingProviderFactory;
        private readonly IEasyCachingProvider provider;
        public CacheService(IEasyCachingProviderFactory easyCachingProviderFactory)
        {
            this.easyCachingProviderFactory = easyCachingProviderFactory;
            provider = easyCachingProviderFactory.GetCachingProvider("mem");
        }

        public async Task<T?> GetAsync(string key)
        {
            var r = await provider.GetAsync<T>(key);
            return r.HasValue ? r.Value : null;
        }

        public async Task<T?> GetByRefKey(string key, string refKey)
        {
            var @ref = await provider.GetAsync<string>(key);
            if (@ref.HasValue)
            {
                var r = await provider.GetAsync<T>(refKey);
                return r.HasValue ? r.Value : null;
            }
            return null;

        }

        public async Task<T?> GetOrSetAsync(string key, Func<Task<T>> acquire, TimeSpan? duration = null)
        {
            var defaultDuration = TimeSpan.FromMinutes(5);
            var value = await GetAsync(key);
            if (value == null)
            {
                var newValue = await acquire();
                if(newValue!= null)
                {
                    await SetAsync(key, newValue, duration ?? defaultDuration);
                    return newValue;
                }
            }
            return value;
        }

        public Task RemoveAsync(string key, List<string>? refKeys)
        {
            var tasks = new List<Task>();
            tasks.Add(provider.RemoveAsync(key));
            if (refKeys != null)
            {
                foreach (var refKey in refKeys)
                {
                    tasks.Add(provider.RemoveAsync(refKey));
                }
            }
            return Task.WhenAll(tasks);
        }

        public Task SetAsync(string key, T value, TimeSpan? duration = null)
        {
            var defaultDuration = TimeSpan.FromMinutes(5);
            return provider.SetAsync(key, value, duration ?? defaultDuration);
        }
    }
}

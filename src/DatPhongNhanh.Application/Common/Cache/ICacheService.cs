namespace DatPhongNhanh.Application.Common.Cache
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefixKey);
    }

    public interface ICachePolicy
    {
        TimeSpan? GetExpirationTime<T>();
        string GenerateKey<T>(params object[] keyParameters);
    }

    public interface ICacheableQuery
    {
        // Cho phép bỏ qua cache khi cần
        bool BypassCache { get; set; }
         string GetCacheKey();

         TimeSpan? Expiration { get; }

    }
}

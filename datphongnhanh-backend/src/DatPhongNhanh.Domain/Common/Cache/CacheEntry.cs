namespace DatPhongNhanh.Domain.Common.Cache;

public interface ICacheEntry
{
    string Key { get;  }
    DateTime? ExpirationTime { get; }
}

public class CacheEntry<T> : ICacheEntry
{
    public string Key { get; private set; }
    public DateTime? ExpirationTime { get; set; }
    public T Value { get; private set; }

    public CacheEntry(string key, T value, TimeSpan? expiration = null)
    {
        Key = key;
        Value = value;
        ExpirationTime = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : null;
    }
}

namespace DatPhongNhanh.SharedKernel.LazyServiceCore.Cache;

public interface ICachedServiceProviderBase : IKeyedServiceProvider
{
    T GetService<T>(T defaultValue);

    object GetService(Type serviceType, object defaultValue);

    T GetService<T>(Func<IServiceProvider, object> factory);

    object GetService(Type serviceType, Func<IServiceProvider, object> factory);
}

public abstract class CachedServiceProviderBase : ICachedServiceProviderBase
{
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<ServiceIdentifier, Lazy<object?>> CachedServices { get; }

    protected CachedServiceProviderBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedServices = new ConcurrentDictionary<ServiceIdentifier, Lazy<object?>>();
        CachedServices.TryAdd(new ServiceIdentifier(typeof(IServiceProvider)), new Lazy<object?>(() => ServiceProvider));
    }

    public virtual object? GetService(Type serviceType)
    {
        return CachedServices.GetOrAdd(
            new ServiceIdentifier(serviceType),
            _ => new Lazy<object?>(() => ServiceProvider.GetService(serviceType))
        ).Value;
    }

    public T GetService<T>(T defaultValue)
    {
        return (T)GetService(typeof(T), defaultValue!);
    }

    public object GetService(Type serviceType, object defaultValue)
    {
        return GetService(serviceType) ?? defaultValue;
    }

    public T GetService<T>(Func<IServiceProvider, object> factory)
    {
        return (T)GetService(typeof(T), factory);
    }

    public object GetService(Type serviceType, Func<IServiceProvider, object> factory)
    {
        return CachedServices.GetOrAdd(
            new ServiceIdentifier(serviceType),
            _ => new Lazy<object?>(() => factory(ServiceProvider))
        ).Value!;
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        return CachedServices.GetOrAdd(
            new ServiceIdentifier(serviceKey, serviceType),
            _ => new Lazy<object?>(() => ServiceProvider.GetKeyedService(serviceType, serviceKey))
        ).Value;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        return CachedServices.GetOrAdd(
            new ServiceIdentifier(serviceKey, serviceType),
            _ => new Lazy<object?>(() => ServiceProvider.GetRequiredKeyedService(serviceType, serviceKey))
        ).Value!;
    }
}

public interface ICachedServiceProvider : ICachedServiceProviderBase
{

}

public interface ICachedTransparentServiceProvider : ICachedServiceProviderBase
{

}

public class CachedServiceProvider : CachedServiceProviderBase, ICachedServiceProvider
{
    public CachedServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }
}

public class CachedTransparentServiceProvider : CachedServiceProviderBase, ICachedTransparentServiceProvider
{
    public CachedTransparentServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
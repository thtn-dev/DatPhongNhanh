using DatPhongNhanh.Application.Common.Behaviours;
using DatPhongNhanh.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatPhongNhanh.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            cfg.RegisterServicesFromAssemblyContaining<DomainAssemblyRef>();

        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));

        return services;
    }
}
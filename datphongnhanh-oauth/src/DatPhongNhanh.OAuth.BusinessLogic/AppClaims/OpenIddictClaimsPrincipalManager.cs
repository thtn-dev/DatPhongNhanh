using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Business.AppClaims;

public sealed class OpenIddictClaimsPrincipalManager(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<OpenIddictClaimsPrincipalOptions> options)
{
    private IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;
    private IOptions<OpenIddictClaimsPrincipalOptions> Options { get; } = options;
    
    public async Task HandleAsync(OpenIddictRequest openIddictRequest, ClaimsPrincipal principal)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        foreach (var providerType in Options.Value.ClaimsPrincipalHandlers)
        {
            var provider = (IOpenIddictClaimsPrincipalHandler)scope.ServiceProvider.GetRequiredService(providerType);
            await provider.HandleAsync(new OpenIddictClaimsPrincipalHandlerContext(scope.ServiceProvider, openIddictRequest, principal));
        }
    }
}
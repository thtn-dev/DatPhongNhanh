using System.Collections.Immutable;
using DatPhongNhanh.OAuth.Business.AppClaims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

public abstract class OAuthControllerBase(IServiceProvider sp) : Controller
{
    private IServiceProvider ServiceProvider { get; } = sp;

    protected IOpenIddictApplicationManager ApplicationManager =>
        ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    protected IOpenIddictAuthorizationManager AuthorizationManager =>
        ServiceProvider.GetRequiredService<IOpenIddictAuthorizationManager>();
    protected IOpenIddictScopeManager ScopeManager => ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
    protected IOpenIddictTokenManager TokenManager => ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();

    protected OpenIddictClaimsPrincipalManager OpenIddictClaimsPrincipalManager => ServiceProvider.GetRequiredService<OpenIddictClaimsPrincipalManager>();
    
    protected UserManager<ApplicationUser> UserManager => ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    protected SignInManager<ApplicationUser> SignInManager => ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
    protected IOptions<OpenIddictClaimsPrincipalOptions> OpenIddictClaimsPrincipalOptions =>
        ServiceProvider.GetRequiredService<IOptions<OpenIddictClaimsPrincipalOptions>>();

    protected virtual Task<OpenIddictRequest> GetOAuthServerRequestAsync(HttpContext context)
    {
        var request = context.GetOpenIddictServerRequest();
        ArgumentNullException.ThrowIfNull(request);
        return Task.FromResult(request);
    }

    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (resources.Count != 0) return resources;

        await foreach (var resource in ScopeManager.ListResourcesAsync(scopes)) resources.Add(resource);
        return resources;
    }

    protected virtual async Task<bool> HasFormValueAsync(string name)
    {
        if (!Request.HasFormContentType) return false;
        var form = await Request.ReadFormAsync();
        return !string.IsNullOrEmpty(form[name]);
    }
}
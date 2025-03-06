using System.Security.Claims;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Business.AppClaims;

public class OpenIddictClaimsPrincipalHandlerContext(
    IServiceProvider scopeServiceProvider,
    OpenIddictRequest openIddictRequest,
    ClaimsIdentity claimsIdentity)
{
    public IServiceProvider ScopeServiceProvider { get; } = scopeServiceProvider;

    public OpenIddictRequest OpenIddictRequest { get; } = openIddictRequest;

    // public ClaimsPrincipal Principal { get; } = principal;
    public ClaimsIdentity Identity { get; }  = claimsIdentity;
}
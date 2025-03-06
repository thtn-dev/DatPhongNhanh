using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

public sealed partial class TokenController
{
    private async Task<IActionResult> HandleClientCredentialsAsync(OpenIddictRequest request,
        CancellationToken cancellationToken = default)
    {
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        ArgumentNullException.ThrowIfNull(application);

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);

        // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
        identity.SetClaim(OpenIddictConstants.Claims.Subject,
            await ApplicationManager.GetClientIdAsync(application, cancellationToken));
        identity.SetClaim(OpenIddictConstants.Claims.Name,
            await ApplicationManager.GetDisplayNameAsync(application, cancellationToken));
        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e. specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        identity.SetScopes(request.GetScopes());
        identity.SetResources(await GetResourcesAsync(request.GetScopes()));

        // handle the token request
        await OpenIddictClaimsPrincipalManager.HandleAsync(request, identity);
        var principal = new ClaimsPrincipal(identity);


        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
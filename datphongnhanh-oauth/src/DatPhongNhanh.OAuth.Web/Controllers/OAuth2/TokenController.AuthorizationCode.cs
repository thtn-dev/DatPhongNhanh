using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

public partial class TokenController
{
    private async Task<IActionResult> HandleAuthorizationCodeAsync(OpenIddictRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var userId = result.Principal?.GetClaim(Claims.Subject);

        if (!result.Succeeded || userId == null)
        {
            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.InvalidGrant,
                ErrorDescription = "The token is invalid."
            });
        }
        var user = await UserManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                }));
        }

        // Ensure the user is still allowed to sign in.
        if (!await SignInManager.CanSignInAsync(user))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                }));
        }
        var identity = new ClaimsIdentity(result.Principal.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

        // changed since the authorization code/refresh token was issued.
        identity.SetClaim(Claims.Subject, await UserManager.GetUserIdAsync(user))
                .SetClaim(Claims.Email, await UserManager.GetEmailAsync(user))
                .SetClaim(Claims.Name, await UserManager.GetUserNameAsync(user))
                .SetClaim(Claims.PreferredUsername, await UserManager.GetUserNameAsync(user))
                .SetClaims(Claims.Role, [.. (await UserManager.GetRolesAsync(user))]);

        await OpenIddictClaimsPrincipalManager.HandleAsync(request, identity);
        var principal = new ClaimsPrincipal(identity);
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}

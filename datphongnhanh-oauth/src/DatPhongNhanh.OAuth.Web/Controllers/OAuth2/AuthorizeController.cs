using System.Security.Claims;
using DatPhongNhanh.OAuth.Web.Extensions;
using DatPhongNhanh.OAuth.Web.Helpers;
using DatPhongNhanh.OAuth.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizeController(IServiceProvider sp) : OAuthControllerBase(sp)
{
    [HttpGet, HttpPost, IgnoreAntiforgeryToken]
    [Route("~/connect/authorize")]
    public async Task<IActionResult> HandleAsync()
    {
        var request = await GetOAuthServerRequestAsync(HttpContext);
        var cancellationToken = HttpContext.RequestAborted;
        // If prompt=login was specified by the client application,
        // immediately return the user agent to the login page.
        if (request.HasPromptValue(OpenIddictConstants.PromptValues.Login))
        {
            // To avoid endless login -> authorization redirects, the prompt=login flag
            // is removed from the authorization request payload before redirecting the user.
            var prompt = string.Join(" ", request.GetPromptValues().Remove(OpenIddictConstants.PromptValues.Login));

            var parameters = Request.HasFormContentType
                ? Request.Form.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList()
                : Request.Query.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(OpenIddictConstants.Parameters.Prompt, new StringValues(prompt)));

            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
                });
        }

        // Retrieve the user principal stored in the authentication cookie.
        // If a max_age parameter was provided, ensure that the cookie is not too old.
        // If the user principal can't be extracted or the cookie is too old, redirect the user to the login page.
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        var inValid = !result.Succeeded ||
                      (request.MaxAge != null && result.Properties?.IssuedUtc != null
                                              && DateTimeOffset.UtcNow - result.Properties.IssuedUtc >
                                              TimeSpan.FromSeconds(request.MaxAge.Value));
        if (inValid)
        {
            // If the client application requested prompt less authentication,
            // return an error indicating that the user is not logged in.
            if (request.HasPromptValue(OpenIddictConstants.PromptValues.None))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                            OpenIddictConstants.Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                    }));
            }

            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? [.. Request.Form] : [.. Request.Query])
                });
        }

        if (result.Principal == null)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? [.. Request.Form] : [.. Request.Query])
                });
        }

        var user = await UserManager.GetUserAsync(result.Principal);
        if (user == null)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? [.. Request.Form] : [.. Request.Query])
                });
        }

        // Retrieve the application details from the database.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found");

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await AuthorizationManager.FindAsync(
            subject: await UserManager.GetUserIdAsync(user),
            client: await ApplicationManager.GetIdAsync(application, cancellationToken),
            status: OpenIddictConstants.Statuses.Valid,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: request.GetScopes(), cancellationToken: cancellationToken).ToListAsync();

        switch (await ApplicationManager.GetConsentTypeAsync(application, cancellationToken))
        {
            // If the consent is external (e.g. when authorizations are granted by a sysadmin),
            // immediately return an error if no authorization can be found in the database.
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count == 0:
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                            OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            }
            // If the consent is implicit or if an authorization was found,
            // return an authorization response without displaying the consent form.
            case OpenIddictConstants.ConsentTypes.Implicit:
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count is not 0:
            case OpenIddictConstants.ConsentTypes.Explicit when authorizations.Count is not 0 &&
                                                                !request.HasPromptValue(OpenIddictConstants.PromptValues.Consent):
            {
                // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: OpenIddictConstants.Claims.Name,
                    roleType: OpenIddictConstants.Claims.Role);

                // Add the claims that will be persisted in the tokens.
                identity.SetClaim(OpenIddictConstants.Claims.Subject, await UserManager.GetUserIdAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.Email, await UserManager.GetEmailAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.Name, await UserManager.GetUserNameAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await UserManager.GetUserNameAsync(user))
                    .SetClaims(OpenIddictConstants.Claims.Role, [.. (await UserManager.GetRolesAsync(user))]);
                
                // Note: in this sample, the granted scopes match the requested scope,
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                identity.SetScopes(request.GetScopes());
                identity.SetResources(await ScopeManager.ListResourcesAsync(identity.GetScopes(), cancellationToken).ToListAsync());
                
                // Automatically create a permanent authorization to avoid requiring explicit consent
                // for future authorization or token requests containing the same scopes.
                var authorization = authorizations.LastOrDefault();
                authorization ??= await AuthorizationManager.CreateAsync(
                    identity: identity,
                    subject : await UserManager.GetUserIdAsync(user),
                    client  : await ApplicationManager.GetIdAsync(application, cancellationToken) ?? string.Empty,
                    type    : OpenIddictConstants.AuthorizationTypes.Permanent,
                    scopes  : identity.GetScopes());

                identity.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization, cancellationToken));
                await OpenIddictClaimsPrincipalManager.HandleAsync(request, identity);
                var principal = new ClaimsPrincipal(identity);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            // At this point, no authorization was found in the database and an error must be returned
            // if the client application specified prompt=none in the authorization request.
            case OpenIddictConstants.ConsentTypes.Explicit
                when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
            case OpenIddictConstants.ConsentTypes.Systematic
                when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                            OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "Interactive user consent is required."
                    }));

            // In every other case, render the consent form.
            default:
                return View("Authorize", new AuthorizeViewModel
                {
                    ApplicationName = await ApplicationManager.GetDisplayNameAsync(application, cancellationToken),
                    Scope = request.Scope
                });
        }
    }

    [Authorize, FormValueRequired("submit.Accept")]
    [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptAsync()
    {
        var request = await GetOAuthServerRequestAsync(HttpContext);

        // Retrieve the profile of the logged-in user.
        var user = await UserManager.GetUserAsync(User) ??
                   throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the application details from the database.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId!) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found.");

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await AuthorizationManager.FindAsync(
            subject: await UserManager.GetUserIdAsync(user),
            client: await ApplicationManager.GetIdAsync(application),
            status: OpenIddictConstants.Statuses.Valid,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: request.GetScopes()).ToListAsync();

        // Note: the same check is already made in the other action but is repeated
        // here to ensure a malicious user can't abuse this POST-only endpoint and
        // force it to return a valid response without the external authorization.
        if (authorizations.Count is 0 &&
            await ApplicationManager.HasConsentTypeAsync(application, OpenIddictConstants.ConsentTypes.External))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The logged in user is not allowed to access this client application."
                }));
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        // Add the claims that will be persisted in the tokens.
        identity.SetClaim(OpenIddictConstants.Claims.Subject, await UserManager.GetUserIdAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Email, await UserManager.GetEmailAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Name, await UserManager.GetUserNameAsync(user))
            .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await UserManager.GetUserNameAsync(user))
            .SetClaims(OpenIddictConstants.Claims.Role, [.. (await UserManager.GetRolesAsync(user))]);

        // Note: in this sample, the granted scopes match the requested scope,
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        identity.SetScopes(request.GetScopes());
        identity.SetResources(await ScopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        // Automatically create a permanent authorization to avoid requiring explicit consent
        // for future authorization or token requests containing the same scopes.
        var authorization = authorizations.LastOrDefault();
        authorization ??= await AuthorizationManager.CreateAsync(
            identity: identity,
            subject: await UserManager.GetUserIdAsync(user),
            client: await ApplicationManager.GetIdAsync(application) ?? string.Empty,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: identity.GetScopes());

        identity.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization));
        await OpenIddictClaimsPrincipalManager.HandleAsync(request, identity);
        var principal = new ClaimsPrincipal(identity);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    [Authorize, FormValueRequired("submit.Deny")]
    [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
    // Notify OpenIddict that the authorization grant has been denied by the resource owner
    // to redirect the user agent to the client application using the appropriate response_mode.
    public IActionResult Deny() => Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);


    [HttpGet("~/connect/logout")]
    public IActionResult Logout() => View();

    [ActionName(nameof(Logout)), HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await SignInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = Url.Content("~/")
            });
    }
}
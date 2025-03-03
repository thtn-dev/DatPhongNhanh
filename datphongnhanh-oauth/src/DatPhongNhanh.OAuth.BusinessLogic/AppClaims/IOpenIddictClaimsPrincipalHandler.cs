using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Business.AppClaims;

using static OpenIddictConstants;

public interface IOpenIddictClaimsPrincipalHandler
{
    Task HandleAsync(OpenIddictClaimsPrincipalHandlerContext context);
}

public sealed class DefaultOpenIddictClaimsPrincipalHandler : IOpenIddictClaimsPrincipalHandler
{
    public Task HandleAsync(OpenIddictClaimsPrincipalHandlerContext context)
    {
        foreach (var claim in context.Principal.Claims)
            switch (claim.Type)
            {
                case Claims.PreferredUsername:
                case JwtRegisteredClaimNames.UniqueName:
                    claim.SetDestinations(Destinations.AccessToken);
                    if (context.Principal.HasScope(Scopes.Profile))
                        claim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                    break;

                case Claims.Email:
                    claim.SetDestinations(Destinations.AccessToken);
                    if (context.Principal.HasScope(Scopes.Email))
                        claim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                    break;

                case Claims.Role:
                    claim.SetDestinations(Destinations.AccessToken);
                    if (context.Principal.HasScope(Scopes.Roles))
                        claim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                    break;

                case "AspNet.Identity.SecurityStamp": break;

                default:
                    claim.SetDestinations(Destinations.AccessToken);
                    break;
            }

        return Task.CompletedTask;
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

[ApiExplorerSettings(IgnoreApi = true)]
public sealed partial class TokenController(IServiceProvider sp) : OAuthControllerBase(sp)
{
    [HttpPost, HttpGet, IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [Route("~/connect/token")]

    public async Task<IActionResult> Exchange()
    {
        var request = await GetOAuthServerRequestAsync(HttpContext);
        var cancellationToken = HttpContext.RequestAborted;

        if (request.IsClientCredentialsGrantType())
            return await HandleClientCredentialsAsync(request, cancellationToken);

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType()     )
            return await HandleAuthorizationCodeAsync(request, cancellationToken);

        return BadRequest(new OpenIddictResponse
        {
            Error = OpenIddictConstants.Errors.UnsupportedGrantType,
            ErrorDescription = "The specified grant type is not supported."
        });
    }
}
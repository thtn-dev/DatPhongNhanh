using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Web.Controllers.OAuth2;

[Route("connect/token")]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public partial class TokenController(IServiceProvider sp) : OAuthControllerBase(sp)
{
   [HttpGet, HttpPost, Produces("application/json")]
   public async Task<IActionResult> Exchange()
   {
       var request = await GetOAuthServerRequestAsync(HttpContext);
       var cancellationToken = HttpContext.RequestAborted;
       if (request.IsClientCredentialsGrantType())
       {
           return await HandleClientCredentialsAsync(request, cancellationToken);
       }
       
       return BadRequest(new OpenIddictResponse
       {
           Error = OpenIddictConstants.Errors.UnsupportedGrantType,
           ErrorDescription = "The specified grant type is not supported."
       });
   }
}
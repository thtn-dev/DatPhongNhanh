using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;

namespace DatPhongNhanh.OAuth.Web.Extensions;

public static class OpenIddictHttpContextExtensions
{
    public static OpenIddictServerTransaction GetOpenIddictServerTransaction(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var openIddictServerTransaction = context.Features.Get<OpenIddictServerAspNetCoreFeature>()?.Transaction;
        if (openIddictServerTransaction != null)
            return openIddictServerTransaction;
        throw new InvalidOperationException("An OpenIddict transaction cannot be found.");
    }
}

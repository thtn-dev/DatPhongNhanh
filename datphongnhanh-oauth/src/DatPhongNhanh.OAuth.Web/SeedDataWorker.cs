using DatPhongNhanh.OAuth.Data.DbContexts;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static System.Net.Mime.MediaTypeNames;

namespace DatPhongNhanh.OAuth.Web;

public class SeedDataWorker(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("console", cancellationToken) == null)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "console",
                ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                DisplayName = "My client application",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Email
                }
            };

            await manager.CreateAsync(descriptor, cancellationToken);
        }
        
        if (await manager.FindByClientIdAsync("mvc", cancellationToken) == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "mvc",
                ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                ConsentType = ConsentTypes.Explicit,
                DisplayName = "MVC client application",
                RedirectUris =
                {
                    new Uri("https://localhost:44338/callback/login/local")
                },
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:44338/callback/logout/local")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman",
                ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                ConsentType = ConsentTypes.Explicit,
                DisplayName = "Postman Client",
                RedirectUris =
                {
                    new Uri("https://oauth.pstmn.io/v1/callback")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
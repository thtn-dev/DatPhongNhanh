using DatPhongNhanh.OAuth.Data.DbContexts;
using OpenIddict.Abstractions;

namespace DatPhongNhanh.OAuth.Web;

public class SeedDataWorker(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var application = await manager.FindByClientIdAsync("console", cancellationToken);
        
        if (application == null)
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
        else
        {
            // delete the client
            await manager.DeleteAsync(application, cancellationToken);
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
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
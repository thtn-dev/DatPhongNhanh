using DatPhongNhanh.OAuth.Data.Entities.Identity;
using DatPhongNhanh.OAuth.Data.Entities.OpenIddict;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.OAuth.Data.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, long>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOpenIddict<ApplicationEntity, AuthorizationEntity, ScopeEntity, TokenEntity, long>();
    }
}

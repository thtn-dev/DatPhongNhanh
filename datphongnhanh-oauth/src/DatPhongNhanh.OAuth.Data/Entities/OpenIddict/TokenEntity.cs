using OpenIddict.EntityFrameworkCore.Models;

namespace DatPhongNhanh.OAuth.Data.Entities.OpenIddict;

public class TokenEntity : OpenIddictEntityFrameworkCoreToken<long, ApplicationEntity, AuthorizationEntity>,
    IEntityBase<long>
{
}
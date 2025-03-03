using OpenIddict.EntityFrameworkCore.Models;

namespace DatPhongNhanh.OAuth.Data.Entities.OpenIddict;

public class AuthorizationEntity : OpenIddictEntityFrameworkCoreAuthorization<long, ApplicationEntity, TokenEntity>,
    IEntityBase<long>
{
}
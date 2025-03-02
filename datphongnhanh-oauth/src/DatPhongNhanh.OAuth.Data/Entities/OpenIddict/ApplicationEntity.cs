using OpenIddict.EntityFrameworkCore.Models;

namespace DatPhongNhanh.OAuth.Data.Entities.OpenIddict;

public class ApplicationEntity : OpenIddictEntityFrameworkCoreApplication<long, AuthorizationEntity, TokenEntity>, IEntityBase<long>
{
    
}
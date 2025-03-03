using Microsoft.AspNetCore.Identity;

namespace DatPhongNhanh.OAuth.Data.Entities.Identity;

public class ApplicationUser : IdentityUser<long>, IEntityBase<long>
{
}
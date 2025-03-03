using Microsoft.AspNetCore.Identity;

namespace DatPhongNhanh.OAuth.Data.Entities.Identity;

public class ApplicationRole : IdentityRole<long>, IEntityBase<long>
{
}
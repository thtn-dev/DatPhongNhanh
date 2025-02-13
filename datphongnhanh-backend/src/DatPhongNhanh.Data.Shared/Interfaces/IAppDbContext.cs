using DatPhongNhanh.Data.Shared.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.Data.Shared.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<UserEntity> Users { get; set; }
    }
}

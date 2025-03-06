using DatPhongNhanh.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.Data.DbContexts.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<UserEntity> Users { get; set; }
    }
}

using DatPhongNhanh.Data.Shared.Entities.Identity;
using DatPhongNhanh.Data.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.Data.Shared.DbContexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<UserEntity> Users { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

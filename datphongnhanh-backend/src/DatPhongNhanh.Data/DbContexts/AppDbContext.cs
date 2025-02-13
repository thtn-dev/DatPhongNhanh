using DatPhongNhanh.Data.DbContexts.Interfaces;
using DatPhongNhanh.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
namespace DatPhongNhanh.Data.DbContexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<UserEntity> Users { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users")
                    .HasKey(x => x.Id);
            });
        }
    }
}

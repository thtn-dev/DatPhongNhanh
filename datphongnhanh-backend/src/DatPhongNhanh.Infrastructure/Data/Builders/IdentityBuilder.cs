using DatPhongNhanh.Domain.Common.Constants;
using DatPhongNhanh.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatPhongNhanh.Infrastructure.Data.Builders;
public static partial class IdentityBuilder
{
    public static void ApplyIdentityModelBuilder(this ModelBuilder builder)
    {
        builder.Entity<UserEntity>(ConfigureUser);

    }
    private static void ConfigureUser(EntityTypeBuilder<UserEntity> entity)
    {
        entity.ToTable("Users", DbSchemaConstants.Default)
            .HasKey(x => x.Id);

        entity
            .Property(x => x.Id)
            .HasColumnName("UserId")
            .IsRequired()
            .HasColumnType(DbTypeConstants.BIGINT);

        entity
            .Property(x => x.UserName)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR256)
            .HasMaxLength(256);

        entity
            .Property(x => x.NormalizedUserName)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR256)
            .HasMaxLength(256);

        entity.HasIndex(x => x.NormalizedUserName)
            .HasAnnotation("idx_username_unique", true)
            .IsUnique();

        entity
            .Property(x => x.Email)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.Email)
            .HasMaxLength(256);

        entity
            .Property(x => x.NormalizedEmail)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR256)
            .HasMaxLength(256);

        entity.HasIndex(x => x.NormalizedEmail)
            .HasAnnotation("idx_email_unique", true)
            .IsUnique();

        entity
            .Property(x => x.PasswordHash)
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR128)
            .HasMaxLength(128);

        entity
            .Property(x => x.PhoneNumber)
            .IsUnicode()
            .HasColumnType(DbTypeConstants.PhoneNumber)
            .HasMaxLength(20);

        entity
            .Property(x => x.SecurityStamp)
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR128)
            .HasMaxLength(128)
            .IsRequired(false);

        entity
            .Property(x => x.ConcurrencyStamp)
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR128)
            .HasMaxLength(128)
            .IsRequired(false);

        entity
            .Property(x => x.CreatedAt)
            .IsRequired();

        entity
            .Property(x => x.UpdatedAt)
            .IsRequired(false)
            .HasDefaultValue(null);

    }
}
using DatPhongNhanh.Domain.Common.Constants;
using DatPhongNhanh.Domain.Homestay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatPhongNhanh.Infrastructure.Data.Builders;
public static partial class HomestayBuilder
{
    public static void ApplyHomestayModelBuilder(this ModelBuilder builder)
    {
        builder.Entity<HomestayEntity>(ConfigureHomestay);
    }

    private static void ConfigureHomestay(EntityTypeBuilder<HomestayEntity> entity)
    {
        entity.ToTable("Homestays", DbSchemaConstants.Default)
            .HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("HomestayId")
            .IsRequired()
            .HasColumnType(DbTypeConstants.BIGINT);

        entity.Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.VARCHAR256)
            .HasMaxLength(256);

        entity.Property(x => x.Description)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(DbTypeConstants.TEXT);

        entity.Property(x => x.ExtraData)
            .HasColumnType(DbTypeConstants.JSON)
            .HasConversion(v => v == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(v), v => v );

        entity.Property(x => x.Longtitude)
            .IsRequired()
            .HasColumnType(DbTypeConstants.DOUBLE_PRECISION);

        entity.Property(x => x.Latitude)
            .IsRequired()
            .HasColumnType(DbTypeConstants.DOUBLE_PRECISION);

        entity.Property(x => x.Geom)
            .IsRequired(false)
            .HasColumnType(DbTypeConstants.GEOMETRY_POINT_3857);  
        
        entity.HasIndex(x => x.Geom)
            .HasDatabaseName("IX_Homestays_Geom")
            .HasMethod("GIST");

        entity.Property(x => x.RawAddress)
            .IsRequired()
            .HasColumnType(DbTypeConstants.VARCHAR512)
            .HasMaxLength(512);
    }
}

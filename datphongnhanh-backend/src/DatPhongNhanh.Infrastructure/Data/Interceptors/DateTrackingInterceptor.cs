using DatPhongNhanh.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DatPhongNhanh.Infrastructure.Data.Interceptors;
public sealed class DateTrackingInterceptor : SaveChangesInterceptor
{
    public DateTrackingInterceptor()
    {

    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateDateTrackingProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateDateTrackingProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateDateTrackingProperties(DbContext? dbContext)
    {
        if (dbContext == null) return;

        var entries = dbContext.ChangeTracker.Entries().Where(x => x.Entity is IDateTracking);

        foreach (var entry in entries)
        {
            var entity = (IDateTracking)entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
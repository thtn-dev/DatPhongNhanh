using DatPhongNhanh.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DatPhongNhanh.Infrastructure.Data.Interceptors;

public sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DispatchDomainEventsInterceptor(IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        return SavedChangesAsync(eventData, result).GetAwaiter().GetResult();
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext? dbContext)
    {
        if (dbContext == null) return;

        var entityEntries = dbContext.ChangeTracker.Entries<HasDomainEventBase>()
            .Select(x => x.Entity)
            .ToList();
        await _dispatcher.DispatchAndClearEvents(entityEntries).ConfigureAwait(false);
    }
}
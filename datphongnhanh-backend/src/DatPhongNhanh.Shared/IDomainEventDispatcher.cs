using DatPhongNhanh.Shared.Entities;


namespace DatPhongNhanh.Shared;
public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents<TId>(IEnumerable<EntityAggregateBase<TId>> entitiesWithEvents) where TId : IEquatable<TId>;

    Task DispatchAndClearEvents(IEnumerable<HasDomainEventBase> domainEvents);
}
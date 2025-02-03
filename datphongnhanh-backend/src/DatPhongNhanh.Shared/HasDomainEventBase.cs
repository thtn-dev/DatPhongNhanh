using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongNhanh.Shared;
public abstract class HasDomainEventBase
{
    private readonly List<DomainEventBase> _domainEvents = [];
    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RegisterDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
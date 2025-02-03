using DatPhongNhanh.Shared;
using MediatR;

namespace DatPhongNhanh.Domain.Homestay.Events;

public record CreateHomestayEvent : DomainEventBase
{
    public long HomestayId { get; }
    public CreateHomestayEvent(long homestayId)
    {
        HomestayId = homestayId;
    }
}


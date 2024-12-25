using DatPhongNhanh.Domain.Homestay.Events;
using MediatR;

namespace DatPhongNhanh.Domain.Homestay.Handlers
{
    internal class CreateHomestayEventHandler : INotificationHandler<CreateHomestayEvent>
    {
        public Task Handle(CreateHomestayEvent notification, CancellationToken cancellationToken)
        {
            var debug = notification.HomestayId;
            return Task.CompletedTask;
        }
    }
}

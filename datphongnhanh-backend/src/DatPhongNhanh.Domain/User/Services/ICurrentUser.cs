

using ErrorOr;

namespace DatPhongNhanh.Domain.User.Services
{
    public interface ICurrentUser
    {
        ErrorOr<string> UserId { get; }
    }
}

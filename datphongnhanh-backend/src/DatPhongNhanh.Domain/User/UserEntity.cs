using Ardalis.GuardClauses;
using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.User;

public class UserEntity : EntityAggregateBase<long>, IDateTracking
{
    public string UserName { get; set; } = null!;
    public string NormalizedUserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
    public string SecurityStamp { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static UserEntity Create(long id,string userName, string email)
    {
        Guard.Against.NegativeOrZero(id, nameof(id));
        Guard.Against.NullOrEmpty(userName, nameof(userName));
        Guard.Against.NullOrEmpty(email, nameof(email));

        return new UserEntity
        {
            Id = id,
            UserName = userName,
            Email = email,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
    }


}
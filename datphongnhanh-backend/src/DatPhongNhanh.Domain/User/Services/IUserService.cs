using ErrorOr;

namespace DatPhongNhanh.Domain.User.Services;

public interface IUserService
{
     IQueryable<UserEntity> Users { get; }
    Task<ErrorOr<UserEntity>> SignInAsync(string userName, string password);
    Task<UserEntity?> GetUserByIdAsync(long userId);
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
    Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity user, string password);
    Task<UserEntity> UpdateUserAsync(Guid userId, string email, string password);
    Task DeleteUserAsync(Guid userId);
}
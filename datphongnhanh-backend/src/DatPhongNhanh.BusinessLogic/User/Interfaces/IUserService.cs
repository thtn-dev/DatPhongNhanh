using DatPhongNhanh.Data.Entities.Identity;

namespace DatPhongNhanh.BusinessLogic.User.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(long id);
        Task<UserEntity?> GetUserByNameAsync(string name);
    }
}

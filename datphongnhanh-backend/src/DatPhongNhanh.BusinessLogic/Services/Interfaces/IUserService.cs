using DatPhongNhanh.Data.Entities.Identity;

namespace DatPhongNhanh.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(long id);
        Task<UserEntity?> GetUserByNameAsync(string name);
    }
}

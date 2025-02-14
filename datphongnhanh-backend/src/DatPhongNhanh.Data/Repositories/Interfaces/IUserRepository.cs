using DatPhongNhanh.Data.Entities.Identity;

namespace DatPhongNhanh.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity entity);
        Task<UserEntity?> FindByIdAsync(long id);
        Task<UserEntity?> FindByNameAsync(string name);
    }
}

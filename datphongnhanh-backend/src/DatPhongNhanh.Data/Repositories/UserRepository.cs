using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.Data.Entities.Identity;
using DatPhongNhanh.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(UserEntity entity)
        {
           await _dbContext.Users.AddAsync(entity);
        }

        public async Task<UserEntity?> FindByIdAsync(long id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<UserEntity?> FindByNameAsync(string name)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == name);
        }
    }
}

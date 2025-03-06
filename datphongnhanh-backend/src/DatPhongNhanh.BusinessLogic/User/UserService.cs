using DatPhongNhanh.BusinessLogic.User.Interfaces;
using DatPhongNhanh.Data.Entities.Identity;
using DatPhongNhanh.Data.Repositories.Interfaces;
using DatPhongNhanh.SharedKernel;

namespace DatPhongNhanh.BusinessLogic.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateUserAsync(UserEntity user)
        {
            await _userRepository.AddAsync(user);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public Task<UserEntity?> GetUserByIdAsync(long id)
        {
            return _userRepository.FindByIdAsync(id);
        }

        public Task<UserEntity?> GetUserByNameAsync(string name)
        {
            return _userRepository.FindByNameAsync(name);
        }
    }
}

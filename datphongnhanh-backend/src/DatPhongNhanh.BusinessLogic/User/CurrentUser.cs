using DatPhongNhanh.BusinessLogic.User.Interfaces;
using DatPhongNhanh.Data.Entities.Identity;

namespace DatPhongNhanh.BusinessLogic.User
{
    public class CurrentUser : ICurrentUser<UserEntity, long>
    {
        public UserEntity Get()
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetAsync()
        {
            throw new NotImplementedException();
        }

        public long GetId()
        {
            throw new NotImplementedException();
        }

        public Task<long> GetIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}

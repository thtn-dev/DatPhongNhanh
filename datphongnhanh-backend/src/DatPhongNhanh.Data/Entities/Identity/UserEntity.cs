using DatPhongNhanh.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatPhongNhanh.Data.Entities.Identity
{
    public class UserEntity : EntityBase<long>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}

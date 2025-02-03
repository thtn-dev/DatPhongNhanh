using DatPhongNhanh.Domain.User.Services;

namespace DatPhongNhanh.Infrastructure.User;

internal class UserNormalize : IUserNormalize
{
    public string NormalizeEmail(string email)
       => IUserNormalize.DefaultNormalize(email);

    public string NormalizeUserName(string userName)
        => IUserNormalize.DefaultNormalize(userName);
}
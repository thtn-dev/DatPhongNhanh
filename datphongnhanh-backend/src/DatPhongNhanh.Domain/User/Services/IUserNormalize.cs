namespace DatPhongNhanh.Domain.User.Services;
public interface IUserNormalize
{
    public static string DefaultNormalize(string s) => s.Trim().ToUpperInvariant();

    string NormalizeEmail(string email);

    string NormalizeUserName(string userName);
}
using DatPhongNhanh.Domain.User.Options;
using DatPhongNhanh.Domain.User.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DatPhongNhanh.Infrastructure.User
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasherOptions _options;
        public PasswordHasher(PasswordHasherOptions options)
        {
            _options = options;
        }
        public string HashPassword(string password)
        {
            byte[] salt = new byte[_options.SaltSize];
            _options.GetRandomNumberGenerator().GetBytes(salt);
            byte[] subKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, _options.Iterations, _options.KeySize);

            byte[] outputBytes = new byte[_options.SaltSize + _options.KeySize];
            salt.CopyTo(outputBytes, 0);
            subKey.CopyTo(outputBytes, _options.SaltSize);

            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = hashBytes.AsSpan(0, _options.SaltSize).ToArray();
            byte[] subKey = hashBytes.AsSpan(_options.SaltSize).ToArray();

            byte[] newSubKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, _options.Iterations, _options.KeySize);

            return newSubKey.SequenceEqual(subKey);
        }
    }
}

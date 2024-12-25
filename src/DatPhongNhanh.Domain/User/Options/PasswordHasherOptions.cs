using System.Security.Cryptography;

namespace DatPhongNhanh.Domain.User.Options
{
    public class PasswordHasherOptions
    {
        public static RandomNumberGenerator RandomNumberGenerator { get; } = RandomNumberGenerator.Create();
        public int Iterations { get; set; }
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
        public RandomNumberGenerator GetRandomNumberGenerator() => RandomNumberGenerator;

        public PasswordHasherOptions()
        {
            Iterations = 10_000;
            SaltSize = 16;
            KeySize = 32;
        }

        public PasswordHasherOptions(int iterations, int saltSize, int keySize)
        {
            Iterations = iterations;
            SaltSize = saltSize;
            KeySize = keySize;
        }

        public static PasswordHasherOptions Default => new();
    }
}

using DatPhongNhanh.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace DatPhongNhanh.Domain.User;

public class RefeshTokenEntity : EntityBase<string>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; } = null!;
    public DateTime Created { get; set; }
    public string RevokedByIp { get; set; } = null!;
    public DateTime? Revoked { get; set; } = null!;
    public string ReplacedByToken { get; set; } = null!;

    [NotMapped]
    public bool IsActive => Revoked == null && !IsExpired;
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= Expires;

    public static string GenerateRFToken()
    {
        // generate a 64-byte array using a secure random number generator
        byte[] randomBytes = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
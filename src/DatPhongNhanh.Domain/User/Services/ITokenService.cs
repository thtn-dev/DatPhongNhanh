
using System.Security.Claims;

namespace DatPhongNhanh.Domain.User.Services;
public interface ITokenService
{
    Task<string> GenerateRefreshToken(string userId, string ipAddress);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> RemoveRefreshToken(string refreshToken);
    Task<bool> RemoveRefreshTokens(string userId);
}
using System.Security.Claims;

namespace DatPhongNhanh.Domain.User.Services;

public interface IJwtService
{
    Task<string> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
}
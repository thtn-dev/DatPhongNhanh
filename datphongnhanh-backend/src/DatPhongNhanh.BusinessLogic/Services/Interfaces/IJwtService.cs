using System.Security.Claims;

namespace DatPhongNhanh.BusinessLogic.Services.Interfaces;
public interface IJwtService
{
    Task<string> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
}
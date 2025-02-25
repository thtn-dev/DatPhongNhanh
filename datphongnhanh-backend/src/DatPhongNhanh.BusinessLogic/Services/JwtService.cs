using DatPhongNhanh.BusinessLogic.Common.Settings;
using DatPhongNhanh.BusinessLogic.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatPhongNhanh.BusinessLogic.Services;
public sealed class JwtService : IJwtService
{

    private readonly JwtSettings _jwtSetting;
    public JwtService(JwtSettings jwtSetting)
    {
        _jwtSetting = jwtSetting;
    }
    public Task<string> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
    {
        var credentials = GetSigningCertificate();
        var token = GenerateEncryptedToken(credentials, claims);
        return Task.FromResult(token);
    }

    private string GenerateEncryptedToken(SigningCredentials credentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken
        (
            _jwtSetting.Issuer,
            _jwtSetting.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinutes),
            signingCredentials: credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SigningCredentials GetSigningCertificate()
    {
        var secret = Encoding.UTF8.GetBytes(_jwtSetting.Secret);
        var key = new SymmetricSecurityKey(secret);
        var result = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return result;
    }
}
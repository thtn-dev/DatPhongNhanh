using DatPhongNhanh.Domain.Common.Options;
using DatPhongNhanh.Domain.User.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatPhongNhanh.Infrastructure.Services;
public sealed class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;
    public JwtService(JwtSetting jwtSetting)
    {
        _jwtSetting = jwtSetting;
    }
    public Task<string> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
    {
        return Task.FromResult(GenerateEncryptedToken(GetSigningCertificate(), claims));
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
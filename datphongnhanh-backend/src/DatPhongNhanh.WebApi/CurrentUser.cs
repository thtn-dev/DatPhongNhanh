using DatPhongNhanh.Domain.User.Services;
using ErrorOr;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DatPhongNhanh.WebApi
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ErrorOr<string> GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.NameId);
            return string.IsNullOrEmpty(userId) ? Error.Unauthorized("User Unauthorized") : userId;
        }

        ErrorOr<string> ICurrentUser.UserId => GetUserId();
    }
}

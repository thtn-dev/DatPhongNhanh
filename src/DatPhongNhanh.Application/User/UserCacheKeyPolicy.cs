using DatPhongNhanh.Application.Common.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatPhongNhanh.Application.User
{
    public class UserCacheKeyPolicy
    {
        private readonly CacheKeyPolicy _policy;

        public const string Prefix = "user";
        public UserCacheKeyPolicy()
        {
            _policy = new CacheKeyPolicy(Prefix)
                .AddKeyGenerator("id", value => $"main:{value}")
                .AddKeyGenerator("email", value => $"email:{value}")
                .AddKeyGenerator("username", value => $"username:{value}");
        }

        public string? GetMainKey(string id) 
        {
            return _policy.TryGetKey(out var key, "id", id) ? key : string.Empty;
        }
        
        public string? GetEmailKey(string email) 
        {
            return _policy.TryGetKey(out var key, "email", email) ? key : string.Empty;
        }

        public string? GetUsernameKey(string username) 
        {
            return _policy.TryGetKey(out var key, "username", username) ? key : string.Empty;
        }

    }
}

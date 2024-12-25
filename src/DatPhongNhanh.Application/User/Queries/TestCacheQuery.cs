using DatPhongNhanh.Application.Common.Cache;
using DatPhongNhanh.Domain.User.Dto;
using DatPhongNhanh.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatPhongNhanh.Application.User.Queries
{
    public class TestCacheQuery : IQuery<ErrorOr<UserLoginResultDto>>, ICacheableQuery
    {
        private bool _bypassCache;
        public bool BypassCache
        {
            get => _bypassCache;
            set
            {
                _bypassCache = value;
            }
        }
        public bool IsThrow { get; set; } = false;

        public string GetCacheKey()
        {
            return $"{nameof(TestCacheQuery)}_{IsThrow}";
        }

        public TimeSpan? Expiration => TimeSpan.FromSeconds(5);
    }

    public class TestCacheQueryHandler : IQueryHandler<TestCacheQuery, ErrorOr<UserLoginResultDto>>
    {
        public async Task<ErrorOr<UserLoginResultDto>> Handle(TestCacheQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            if (request.IsThrow)
            {
                return Error.Forbidden("Test throw exception");
            }
            var randomNumber = new Random().Next(1, 100);
            return new UserLoginResultDto
            {
                Token = randomNumber.ToString(),
                Sub = "aaaa"+ " " + randomNumber.ToString(),
            };
        }
    }
}

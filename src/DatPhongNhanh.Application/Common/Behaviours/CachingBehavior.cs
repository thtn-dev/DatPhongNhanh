using DatPhongNhanh.Application.Common.Cache;

namespace DatPhongNhanh.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheableQuery
        where TResponse : class, IErrorOr
    {

        private readonly ICacheService _cache;
        private readonly ICachePolicy _cachePolicy;

        public CachingBehavior(ICacheService cache, ICachePolicy cachePolicy)
        {
            _cache = cache;
            _cachePolicy = cachePolicy;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                if (request.BypassCache)
                {
                    return await next();
                }

                var key = request.GetCacheKey();
                Type resultType = typeof(TResponse).GetGenericArguments()[0];

                var getCacheAsyncMethod = _cache.GetType()
                    .GetMethod(nameof(ICacheService.GetAsync))?
                    .MakeGenericMethod(resultType);

                var task = getCacheAsyncMethod!.Invoke(_cache, [key])!;

                await (Task)task;

                var cachedValue = task.GetType()
                    .GetProperty(nameof(Task<object>.Result))?
                    .GetValue(task);

                if (cachedValue != null)
                {
                    var errorOrType = typeof(ErrorOrFactory)
                        .GetMethod(nameof(ErrorOrFactory.From))?
                        .MakeGenericMethod(resultType)!;
                    return (TResponse)errorOrType.Invoke(null, [cachedValue])!;
                }

                var response = await next();

                if (!response.IsError)
                {

                    var successValue = response.GetType()
                        .GetProperty(nameof(ErrorOr<object>.Value))?
                        .GetValue(response);

                    var setCacheAsyncMethod = _cache.GetType()
                        .GetMethod(nameof(ICacheService.SetAsync))?
                        .MakeGenericMethod(resultType);

                    var expiration = request.Expiration;
                    var setCacheTask = setCacheAsyncMethod!.Invoke(_cache, [key, successValue, expiration])!;
                    await (Task)setCacheTask;
                }
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviours;

public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingBehaviour<TRequest, TResponse>> _logger;

    public CachingBehaviour(ICacheService cacheService, ILogger<CachingBehaviour<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only cache queries (requests that don't modify data)
        if (request is not ICacheableQuery cacheableQuery)
        {
            return await next();
        }

        var cacheKey = cacheableQuery.CacheKey;
        if (string.IsNullOrEmpty(cacheKey))
        {
            return await next();
        }

        // Try to get from cache first
        var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey);
        if (cachedResponse != null)
        {
            _logger.LogInformation("Returning cached response for {RequestType} with key {CacheKey}", typeof(TRequest).Name, cacheKey);
            return cachedResponse;
        }

        // If not in cache, execute the request
        var response = await next();

        // Cache the response
        if (response != null)
        {
            var expiration = cacheableQuery.CacheExpiration ?? TimeSpan.FromMinutes(5);
            await _cacheService.SetAsync(cacheKey, response, expiration);
            _logger.LogInformation("Cached response for {RequestType} with key {CacheKey}", typeof(TRequest).Name, cacheKey);
        }

        return response;
    }
} 
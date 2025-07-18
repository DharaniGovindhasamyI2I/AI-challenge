using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting value from cache for key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var jsonValue = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            await _cache.SetStringAsync(key, jsonValue, options);
            _logger.LogDebug("Cached value for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting value in cache for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            _logger.LogDebug("Removed cache entry for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache entry for key: {Key}", key);
        }
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        try
        {
            // Note: This is a simplified implementation
            // In a real scenario, you might want to use Redis SCAN command
            // or implement a more sophisticated pattern matching
            _logger.LogWarning("Pattern-based cache removal not implemented for key pattern: {Pattern}", pattern);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache entries for pattern: {Pattern}", pattern);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence for key: {Key}", key);
            return false;
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
        {
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return cachedValue;
        }

        _logger.LogDebug("Cache miss for key: {Key}", key);
        var value = await factory();
        await SetAsync(key, value, expiration);
        return value;
    }
} 
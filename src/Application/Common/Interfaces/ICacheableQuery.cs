namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan? CacheExpiration { get; }
} 
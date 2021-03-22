using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;

namespace Microservices.Common.Cache
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public CacheHelper(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public async Task AddAsync(string cacheKey, object data, uint durationInSeconds)
        => await _redisCacheClient.Db0.AddAsync(cacheKey, data, DateTimeOffset.UtcNow.AddSeconds(durationInSeconds));


        public async Task<T> GetAsync<T>(string cacheKey)
        => await _redisCacheClient.Db0.GetAsync<T>(cacheKey);

        public async Task RemoveAsync(string cacheKey)
        => await _redisCacheClient.Db0.RemoveAsync(cacheKey);

        public async Task RemoveByPatternAsync(string cacheKeyStartsWith)
        {
            var keys = await _redisCacheClient.Db0.SearchKeysAsync($"{cacheKeyStartsWith}*");
            await _redisCacheClient.Db0.RemoveAllAsync(keys);
        }
    }
}

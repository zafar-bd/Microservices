using System.Threading.Tasks;

namespace Microservices.Common.Cache
{
    public interface ICacheHelper
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task RemoveAsync(string cacheKey);
        Task RemoveByPatternAsync(string cacheKeyStartsWith);
        Task AddAsync(string cacheKey, object data, uint durationInSeconds);
    }
}

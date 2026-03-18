using System;
using System.Threading.Tasks;

namespace Ecommerce.API.Caching
{
    public class NoopCacheService : ICacheService
    {
        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult<T?>(default);
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            return Task.CompletedTask;
        }
    }
}

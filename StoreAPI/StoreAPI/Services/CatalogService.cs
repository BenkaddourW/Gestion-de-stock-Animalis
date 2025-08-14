using Microsoft.Extensions.Caching.Memory;

namespace StoreAPI.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;
        private const string ApiBaseUrl = "https://localhost:7254/api/Produits";

        public CatalogService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await memoryCache.GetOrCreateAsync("categories_cache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{ApiBaseUrl}/categorie");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
            });
        }

        public async Task<List<string>> GetAnimalTypesAsync()
        {
            return await memoryCache.GetOrCreateAsync("animalTypes_cache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{ApiBaseUrl}/animalTypes");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
            });
        }
    }
}


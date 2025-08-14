namespace StoreAPI.Services
{
    public interface ICatalogService
    {
        
            Task<List<string>> GetCategoriesAsync();
            Task<List<string>> GetAnimalTypesAsync();
        
    }
}

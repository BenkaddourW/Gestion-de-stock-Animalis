using System.Collections.Generic;
using System.Net.Http;
using Azure;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Models.DTO;

namespace StoreAPI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:7254/api/Produits";
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string search = null, string category = null, string animalType = null)
        {
            List<ProduitDto> response = new List<ProduitDto>();
            try
            {
                var client = httpClientFactory.CreateClient();

                // Construction plus robuste de l'URL
                var query = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(search))
                    query["search"] = search;

                if (!string.IsNullOrEmpty(category))
                    query["category"] = category;

                if (!string.IsNullOrEmpty(animalType))
                    query["animalType"] = animalType;

                string apiUrl = query.Any()
                    ? $"{ApiBaseUrl}/filter?{string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"))}"
                    : ApiBaseUrl;

                var httpResponseMessage = await client.GetAsync(apiUrl);

                if (!httpResponseMessage.IsSuccessStatusCode)
                    return View(new List<ProduitDto>());

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProduitDto>>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
            }

            ViewBag.Categories = await GetAllCategoriesAsync();
            ViewBag.AnimalTypes = await GetAnimalTypesAsync();

            return View(response);
        }

       
        private async Task<List<string>> GetAnimalTypesAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{ApiBaseUrl}/animalTypes");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<string>>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task<List<string>> GetAllCategoriesAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{ApiBaseUrl}/categorie");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<string>>();
            }
            catch
            {
                return new List<string>();
            }
        }


        public async Task<IActionResult> ByCategory(string category)
        {
            List<ProduitDto> response = new List<ProduitDto>();
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync($"{ApiBaseUrl}/byCategory/{category}");

                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProduitDto>>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
            }

            return View("Index", response);
        }

       
    }

    
}

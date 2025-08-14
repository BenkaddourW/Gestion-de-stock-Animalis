
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using StoreAPI.Models.DTO;
using StoreAPI.Services;

namespace StoreAPI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICatalogService catalogService;

        //private readonly string _apiBaseUrl = "https://localhost:7254/api/Produits";

        private readonly IHttpClientFactory httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory, ICatalogService catalogService)
        {

            this.httpClientFactory = httpClientFactory;
            this.catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProduitDto> response = new List<ProduitDto>();
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7254/api/Produits");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProduitDto>>());

            }
            catch (Exception ex)
            {
                //log
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Ajouter()
        {
            ViewBag.Categories = await catalogService.GetCategoriesAsync();
            ViewBag.AnimalTypes = await catalogService.GetAnimalTypesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ajouter(AddProduitDto model, IFormFile ImageFile)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7254/api/Produits"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProduitDto>();

            if (response is not null)
            {
                // Envoie de l’image à l’API
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var clientUpload = httpClientFactory.CreateClient();
                    using var form = new MultipartFormDataContent();
                    var streamContent = new StreamContent(ImageFile.OpenReadStream());
                    form.Add(streamContent, "file", $"{model.IdProduit}.jpg");

                    var uploadResponse = await clientUpload.PostAsync(
                        $"https://localhost:7254/api/Produits/upload/{model.IdProduit}", form);

                    uploadResponse.EnsureSuccessStatusCode();
                }

                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Admin");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProduitDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7254/api/Produits/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex) { }

            return View("Admin");
        }
    
    [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<ProduitDto>($"https://localhost:7254/api/Produits/{id.ToString()}");

            ViewBag.Categories = await catalogService.GetCategoriesAsync();
            ViewBag.AnimalTypes = await catalogService.GetAnimalTypesAsync();
            if (response is not null)
            {
                return View(response);

            }
            return View(null);

        }


        [HttpPost]
        //public async Task<IActionResult> Edit(ProduitDto request)
   
        public async Task<IActionResult> Edit(ProduitDto request, IFormFile ImageFile)


        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7254/api/Produits/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProduitDto>();

            if (response is not null)
            {
                // ✅ Envoi de la nouvelle image s’il y en a une
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadClient = httpClientFactory.CreateClient();
                    using var form = new MultipartFormDataContent();
                    var streamContent = new StreamContent(ImageFile.OpenReadStream());
                    form.Add(streamContent, "file", $"{request.IdProduit}.jpg");

                    var uploadResponse = await uploadClient.PostAsync(
                        $"https://localhost:7254/api/Produits/upload/{request.IdProduit}", form);

                    uploadResponse.EnsureSuccessStatusCode();
                }

                TempData["Success"] = "Produit modifié avec succès !";
                return RedirectToAction("Edit", new { id = request.Id });
            }



            return View();

        }

    }
}
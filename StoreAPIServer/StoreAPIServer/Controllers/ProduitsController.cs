using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPIServer.Data;
using StoreAPIServer.Models;
using StoreAPIServer.Models.Entities;

namespace StoreAPIServer.Controllers
{
    //'https://localhost:7254/api/Produits'
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProduitsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public ProduitsController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        //trouver tt les produits

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public IActionResult GetAllProducts()
        {

            return Ok(appDbContext.Produits.ToList());
        }


        // recuperer les  images
        [HttpGet("image/{idProduit}")]
        //[Authorize]
        public IActionResult GetProductImage(int idProduit)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "produits", $"{idProduit}.jpg");

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image non trouvée");
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg");
        }
        //ajouter un produit
        [HttpPost]

        public IActionResult AddProduct(AddProduitDto addProduitDto)
        {
            var produitEntity = new Produit()
            {
                IdProduit = addProduitDto.IdProduit,
                Nom = addProduitDto.Nom,
                Description = addProduitDto.Description,
                AnimalType = addProduitDto.AnimalType,
                Categorie = addProduitDto.Categorie,
                Prix = addProduitDto.Prix,
                Quantite = addProduitDto.Quantite

            };

            appDbContext.Produits.Add(produitEntity);
            appDbContext.SaveChanges();

            return Ok(produitEntity);


        }

        // trouver produit par id
        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize]
        public IActionResult GetProductById(Guid id)
        {
            var produit = appDbContext.Produits.Find(id);
            if (produit == null)
            {
                return NotFound();
            }
            return Ok(produit);
        }

        // trouver produit par type d animal
        [HttpGet]
        [Route("{animalType}")]
        //[Authorize]
        public IActionResult GetProductByAnimalType(string animalType)
        {
            var produits = appDbContext.Produits
                .Where(p => p.AnimalType == animalType)
                .ToList();

            if (produits == null || !produits.Any())
            {
                return NotFound();
            }
            return Ok(produits);
        }
        //mettre a jour par id
        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize]
        public IActionResult UpdateEmployeeById(Guid id, UpdatePoduitDto updatePoduitDto)
        {
            var produit = appDbContext.Produits.Find(id);
            if (produit == null)
            {
                return NotFound();
            }
            produit.IdProduit = updatePoduitDto.IdProduit;
            produit.Nom = updatePoduitDto.Nom;
            produit.Description = updatePoduitDto.Description;
            produit.AnimalType = updatePoduitDto.AnimalType;
            produit.Categorie = updatePoduitDto.Categorie;
            produit.Prix = updatePoduitDto.Prix;
            produit.Quantite = updatePoduitDto.Quantite;
            appDbContext.SaveChanges();
            return Ok(produit);

        }

        ////supprimer par id
        //[HttpDelete]
        //[Route("{id:guid}")]
        ////[Authorize]

        //public IActionResult DeleteProdutById(Guid id)
        //{
        //    var produit = appDbContext.Produits.Find(id);
        //    if (produit == null)
        //    {
        //        return NotFound();
        //    }
        //    appDbContext.Produits.Remove(produit);
        //    appDbContext.SaveChanges();
        //    return Ok(produit);

        //}

        [HttpDelete("{id:guid}")]
        public IActionResult SupprimerProduitAvecSP(Guid id)
        {
            try
            {
                var produit = appDbContext.Produits.Find(id);
                if (produit == null)
                {
                    return NotFound("Produit introuvable.");
                }

                appDbContext.SupprimerProduitViaSP(id);
                return Ok(new { message = "Produit supprimé " });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la suppression : {ex.Message}");
            }
        }


        //pour récupérer tous les types d'animaux disponibles
        [HttpGet("animalTypes")]
        //[Authorize]
        public IActionResult GetAnimalTypes()
        {
            var animalTypes = appDbContext.Produits
                .Select(p => p.AnimalType)
                .Distinct()
                .ToList();

            return Ok(animalTypes);
        }
        //pour récupérer tous les categories disponibles
        [HttpGet("categorie")]
        //[Authorize]
        public IActionResult GetCategorie()
        {
            var categorie = appDbContext.Produits
                .Select(p => p.Categorie)
                .Distinct()
                .ToList();

            return Ok(categorie);
        }



        [HttpGet("byCategorieSql/{categorie}")]
        public IActionResult GetProduitsCategorieSql(string categorie)
        {
            var produits = appDbContext.GetProduitsParCategorie(categorie).ToList();
            return Ok(produits);
        }


        // tt produits par categorie
        //[HttpGet("byCategory/{category}")]
        ////[Authorize]
        //public async Task<ActionResult<IEnumerable<Produit>>> GetByCategory(string category)
        //{
        //    var produits = await appDbContext.Produits
        //        .Where(p => p.Categorie == category)
        //        .ToListAsync();

        //    return Ok(produits);
        //}



        // rechercher par nom et description 
        [HttpGet("search")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Produit>>> SearchProducts([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Le terme de recherche est requis");

            return await appDbContext.Produits
                .Where(p => p.Nom.Contains(term) || p.Description.Contains(term))
                .ToListAsync();
        }


        //filtre
        [HttpGet("filter")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Produit>>> GetFilteredProducts(
    [FromQuery] string search = null,
    [FromQuery] string category = null,
    [FromQuery] string animalType = null)
        {
            var query = appDbContext.Produits.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Nom.Contains(search) || p.Description.Contains(search));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Categorie == category);

            if (!string.IsNullOrEmpty(animalType))
                query = query.Where(p => p.AnimalType == animalType);

            return await query.ToListAsync();
        }
        [HttpPost("upload/{idProduit}")]
        public async Task<IActionResult> UploadImage(int idProduit, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fichier invalide");

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "produits", $"{idProduit}.jpg");

            // Créer le dossier s’il n’existe pas
            var directory = Path.GetDirectoryName(imagePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { success = true, message = "Image enregistrée avec succès" });
        }

    }
}

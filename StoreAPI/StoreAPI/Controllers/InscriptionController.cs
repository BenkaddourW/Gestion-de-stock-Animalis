using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StoreAPI.Controllers
{
    public class InscriptionController : Controller
    {
        private readonly IConfiguration _configuration;

        public InscriptionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Inscription
        public IActionResult Index()
        {
            return View();
        }

        // POST: Inscription/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UtilisateurDto utilisateurDto)
        {
            if (!ModelState.IsValid)
            {
                return View(utilisateurDto);
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("InscrireUtilisateur", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Ajouter les paramètres
                        command.Parameters.AddWithValue("@Prenom", utilisateurDto.Prenom);
                        command.Parameters.AddWithValue("@Nom", utilisateurDto.Nom);
                        command.Parameters.AddWithValue("@Email", utilisateurDto.Email);
                        command.Parameters.AddWithValue("@Telephone", utilisateurDto.Telephone);
                        command.Parameters.AddWithValue("@MotDePasse", utilisateurDto.Password); // Note: Dans une application réelle, vous devriez hasher le mot de passe

                        // Exécuter la procédure stockée
                        var userId = await command.ExecuteScalarAsync();

                        if (userId != null)
                        {
                            // Rediriger vers une page de succès ou de connexion
                            return RedirectToAction("Index", "Login");
                        }
                    }
                }

                // Si nous arrivons ici, quelque chose s'est mal passé
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription.");
                return View(utilisateurDto);
            }
            catch (SqlException ex)
            {
                // Gérer les erreurs spécifiques de SQL
                if (ex.Number == 50000) // C'est le numéro d'erreur pour RAISERROR dans SQL Server
                {
                    ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Une erreur de base de données s'est produite.");
                }
                return View(utilisateurDto);
            }
            catch (Exception ex)
            {
                // Gérer les autres erreurs
                ModelState.AddModelError(string.Empty, "Une erreur inattendue s'est produite.");
                return View(utilisateurDto);
            }
        }
    }
}
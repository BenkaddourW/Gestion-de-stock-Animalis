using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;
using StoreAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StoreAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly StoreContext _context;

        public LoginController(StoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("AuthentifierUtilisateur", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        
                        command.Parameters.AddWithValue("@Email", loginViewModel.Email);
                        command.Parameters.AddWithValue("@MotDePasse", loginViewModel.MotDePasse);

                       
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows && await reader.ReadAsync())
                            {
                                
                                var userId = reader.GetInt32(0);
                                var prenom = reader.GetString(1);
                                var nom = reader.GetString(2);

                                
                                HttpContext.Session.SetInt32("UserId", userId);
                                HttpContext.Session.SetString("Prenom", prenom);
                                HttpContext.Session.SetString("Nom", nom);
                                HttpContext.Session.SetString("Email", loginViewModel.Email);

                               
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }

                
                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
                return View(loginViewModel);
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Une erreur de base de données s'est produite.");
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Une erreur inattendue s'est produite.");
                return View(loginViewModel);
            }
        }

        public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();

            
            return RedirectToAction("Index", "Login");
        }
    }
}
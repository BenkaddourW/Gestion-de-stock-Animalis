using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models.DTO
{
    public class UtilisateurDto
    {
            [Required(ErrorMessage = "L'email est obligatoire")]
            [EmailAddress(ErrorMessage = "Format d'email invalide")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Le mot de passe est obligatoire")]
            [DataType(DataType.Password)]
            [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
            public string Password { get; set; }

            [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Le prénom est obligatoire")]
            public string Prenom { get; set; }

            [Required(ErrorMessage = "Le nom est obligatoire")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le téléphone est obligatoire")]
            [Phone(ErrorMessage = "Format de téléphone invalide")]
            public string Telephone { get; set; }

    }
    }

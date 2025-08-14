using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreAPIServer.Models.Entities
{
    public class Utilisateur
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [NotMapped]
        [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        [Display(Name = "Confirmation du mot de passe")]
        [DataType(DataType.Password)]
        public string ConfirmationMotDePasse { get; set; }


        [Display(Name = "Type d'animal")]
        public string TypeAnimal { get; set; }

        [Display(Name = "Nom de l'animal")]
        public string NomAnimal { get; set; }

        public bool AccepteConditions { get; set; }
        public bool InscritNewsletter { get; set; }
    }
}

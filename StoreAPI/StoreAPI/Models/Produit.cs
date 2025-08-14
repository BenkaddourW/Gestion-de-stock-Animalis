using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
      
        public int IdProduit { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }

        public string AnimalType { get; set; }
        public string Categorie { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
    }
}

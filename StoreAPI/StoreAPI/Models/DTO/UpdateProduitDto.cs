using System.ComponentModel.DataAnnotations;

namespace StoreAPIServer.Models
{
    public class UpdateProduitDto
    {
        public int IdProduit { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }

        public string AnimalType { get; set; }
        public string Categorie { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreAPI.Models.DTO;

namespace StoreAPI.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }
        public DbSet<Produit> Produits { get; set; }
        //public DbSet<UtilisateurDto> Utilisateurs { get; set; }

    }
}

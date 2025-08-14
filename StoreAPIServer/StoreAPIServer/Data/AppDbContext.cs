using Microsoft.EntityFrameworkCore;
using StoreAPIServer.Models;
using StoreAPIServer.Models.Entities;

namespace StoreAPIServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produit> Produits { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public IQueryable<Produit> GetProduitsParCategorie(string categorie)
        {
            return Produits.FromSqlRaw("EXEC GetProduitsByCategorie @p0", categorie);
        }

        public void SupprimerProduitViaSP(Guid id)
        {
            Database.ExecuteSqlRaw("EXEC DeleteProduitById @p0", id);
        }


    }
}
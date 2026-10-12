using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity
{
    public class WarehouseContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
                options.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=Warehouse2026;" +
                    "Integrated Security=True;" +
                    "TrustServerCertificate=True")
                    .UseLazyLoadingProxies();
        }

        public void Seed()
        {
            // Kontrola, zda v databázi již existují nějaké jednotky nebo materiály.
            if (UnitsOfMeasure.Any() || Materials.Any())
            {
                return;
            }

            // Vytvoření měrných jednotek
            var mjKs = new UnitOfMeasure { Description = "ks" };
            var mjKg = new UnitOfMeasure { Description = "kg" };
            var mjM3 = new UnitOfMeasure { Description = "m3" };

            // Vytvoření materiálů s vazbou na objekt měrné jednotky
            var materialSroub = new Material
            {
                Name = "Šroub",
                SafetyStock = 1000,
                PalletCapacity = 2000,
                UnitOfMeasure = mjKs
            };

            var materialHrebik = new Material
            {
                Name = "Hřebík",
                SafetyStock = 1500,
                PalletCapacity = 1000,
                UnitOfMeasure = mjKs
            };

            // Hromadné přidání do DbContextu
            UnitsOfMeasure.AddRange(mjKs, mjKg, mjM3);
            Materials.AddRange(materialSroub, materialHrebik);

            // Jediné finální uložení změn do databáze
            SaveChanges();
        }


    }
}

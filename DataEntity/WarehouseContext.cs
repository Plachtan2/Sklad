using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity
{
    public class WarehouseContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<Pallet> Pallets { get; set; }
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

        public void ClearAndReseed()
        {
            // 1. Smazání dat pomocí surového SQL (postupně od závislých tabulek k hlavním kvůli cizím klíčům)
            Database.ExecuteSqlRaw("DELETE FROM [Pallets]");
            Database.ExecuteSqlRaw("DELETE FROM [Materials]");
            Database.ExecuteSqlRaw("DELETE FROM [UnitsOfMeasure]");

            // 2. Resetování identity (počítadla primárního klíče), aby nová data začínala od ID 1
            Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[Pallets]', RESEED, 0)");
            Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[Materials]', RESEED, 0)");
            Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[UnitsOfMeasure]', RESEED, 0)");
        }


        public void SeedOld()
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

        public void Seed()
        {
            // Ochrana před duplicitami – pokud již v databázi existují jakákoliv data, metoda se ihned ukončí
            if (UnitsOfMeasure.Any() || Materials.Any() || Pallets.Any())
            {
                return;
            }

            // 1. Měrné jednotky
            var mjKs = new UnitOfMeasure { Description = "ks" };
            var mjKg = new UnitOfMeasure { Description = "kg" };
            var mjM3 = new UnitOfMeasure { Description = "m3" };

            UnitsOfMeasure.AddRange(mjKs, mjKg, mjM3);

            // 2. 30 realistických materiálů strojírenského skladu
            var m1 = new Material { Name = "Šroub M8x30", SafetyStock = 1000, PalletCapacity = 2000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-10) };
            var m2 = new Material { Name = "Hřebík 2.5x50", SafetyStock = 1500, PalletCapacity = 1000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-12) };
            var m3 = new Material { Name = "Matice M8", SafetyStock = 2000, PalletCapacity = 5000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-8) };
            var m4 = new Material { Name = "Podložka M8", SafetyStock = 3000, PalletCapacity = 10000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-15) };
            var m5 = new Material { Name = "Vrut do dřeva 4x40", SafetyStock = 800, PalletCapacity = 3000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-5) };
            var m6 = new Material { Name = "Nýt trhací 4x10", SafetyStock = 1200, PalletCapacity = 4000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-20) };
            var m7 = new Material { Name = "Závitová tyč M10", SafetyStock = 200, PalletCapacity = 500, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-3) };
            var m8 = new Material { Name = "Podložka pérová M10", SafetyStock = 1500, PalletCapacity = 6000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-18) };
            var m9 = new Material { Name = "Kolík válcový 6x30", SafetyStock = 500, PalletCapacity = 2500, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-7) };
            var m10 = new Material { Name = "Péro těsnicí 8x7x30", SafetyStock = 300, PalletCapacity = 1500, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-14) };

            var m11 = new Material { Name = "Ocelový plech 2mm", SafetyStock = 500, PalletCapacity = 1500, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-30) };
            var m12 = new Material { Name = "Hliníkový profil 40x40", SafetyStock = 300, PalletCapacity = 800, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-25) };
            var m13 = new Material { Name = "Nerezová trubka 25x2", SafetyStock = 250, PalletCapacity = 600, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-11) };
            var m14 = new Material { Name = "Mosazná tyč 12mm", SafetyStock = 150, PalletCapacity = 400, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-9) };
            var m15 = new Material { Name = "Ocelový nosník IPN 100", SafetyStock = 1000, PalletCapacity = 3000, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-40) };
            var m16 = new Material { Name = "Svařovací drát 1.0mm", SafetyStock = 100, PalletCapacity = 500, UnitOfMeasure = mjKg, ReceivedOn = DateTime.Today.AddDays(-6) };
            var m17 = new Material { Name = "Brusný kotouč 125mm", SafetyStock = 50, PalletCapacity = 200, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-2) };
            var m18 = new Material { Name = "Řezný kotouč 150mm", SafetyStock = 80, PalletCapacity = 300, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-4) };
            var m19 = new Material { Name = "Vrták do kovu 8mm", SafetyStock = 40, PalletCapacity = 150, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-1) };
            var m20 = new Material { Name = "Fréza válcová 10mm", SafetyStock = 20, PalletCapacity = 100, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-13) };

            var m21 = new Material { Name = "Chladicí emulze", SafetyStock = 2, PalletCapacity = 10, UnitOfMeasure = mjM3, ReceivedOn = DateTime.Today.AddDays(-22) };
            var m22 = new Material { Name = "Provozní olej VG 46", SafetyStock = 1, PalletCapacity = 5, UnitOfMeasure = mjM3, ReceivedOn = DateTime.Today.AddDays(-16) };
            var m23 = new Material { Name = "Technický benzín", SafetyStock = 1, PalletCapacity = 4, UnitOfMeasure = mjM3, ReceivedOn = DateTime.Today.AddDays(-19) };
            var m24 = new Material { Name = "Průmyslový odmašťovač", SafetyStock = 2, PalletCapacity = 8, UnitOfMeasure = mjM3, ReceivedOn = DateTime.Today.AddDays(-21) };

            var m25 = new Material { Name = "Kartonová krabice 400x300", SafetyStock = 500, PalletCapacity = 2000, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-28) };
            var m26 = new Material { Name = "Strečová fólie 500mm", SafetyStock = 60, PalletCapacity = 240, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-17) };
            var m27 = new Material { Name = "Vázací páska PP", SafetyStock = 30, PalletCapacity = 100, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-23) };
            var m28 = new Material { Name = "Bublinková fólie", SafetyStock = 5, PalletCapacity = 20, UnitOfMeasure = mjM3, ReceivedOn = DateTime.Today.AddDays(-26) };
            var m29 = new Material { Name = "Dřevěná bedna 800x600", SafetyStock = 20, PalletCapacity = 50, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-29) };
            var m30 = new Material { Name = "Plastová přepravka KLT", SafetyStock = 100, PalletCapacity = 400, UnitOfMeasure = mjKs, ReceivedOn = DateTime.Today.AddDays(-12) };

            Materials.AddRange(
                m1, m2, m3, m4, m5, m6, m7, m8, m9, m10,
                m11, m12, m13, m14, m15, m16, m17, m18, m19, m20,
                m21, m22, m23, m24, m25, m26, m27, m28, m29, m30
            );

            // 3. 30 palet (stavy dle enumu PalletStatus: Stored = 0)
            var pallets = new[]
            {
                // Původní 3 ukázkové palety pro Šroub a Hřebík
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "M10", Quantity = 500, Material = m1, Comment = "Šrouby M8" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "M2", Quantity = 750, Material = m1 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "M2/5", Quantity = 1000, Material = m2 },

                // Dalších 17 obsazených palet (přiřazený materiál a množsví > 0)
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "A-01", Quantity = 2000, Material = m3 },
                new Pallet { Type = PalletType.Large, Status = PalletStatus.Stored, StorageLocation = "A-02", Quantity = 4000, Material = m3 },
                new Pallet { Type = PalletType.Large, Status = PalletStatus.Stored, StorageLocation = "A-03", Quantity = 1500, Material = m3 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "A-04", Quantity = 2000, Material = m3 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "B-01", Quantity = 300, Material = m7 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "B-02", Quantity = 3000, Material = m8 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "B-03", Quantity = 1000, Material = m9 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "B-04", Quantity = 800, Material = m10 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "C-01", Quantity = 1000, Material = m11 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "C-02", Quantity = 500, Material = m11 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "C-03", Quantity = 400, Material = m13 },
                new Pallet { Type = PalletType.Split, Status = PalletStatus.Stored, StorageLocation = "C-04", Quantity = 250, Material = m14 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "D-01", Quantity = 1500, Material = m11 },
                new Pallet { Type = PalletType.Large, Status = PalletStatus.Stored, StorageLocation = "D-02", Quantity = 300, Material = m16 },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "D-03", Quantity = 100, Material = m12 },

                // 5 rezervovaných palet (přiřazený materiál + nulové množství + stav dle enumu PalletStatus)
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "R-01", Quantity = 0, Material = m3, Comment = "Rezervovaná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "R-02", Quantity = 0, Material = m11, Comment = "Rezervovaná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "R-03", Quantity = 0, Material = m13, Comment = "Rezervovaná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "R-04", Quantity = 0, Material = m14, Comment = "Rezervovaná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "R-05", Quantity = 0, Material = m9, Comment = "Rezervovaná paleta" },

                // 5 prázdných palet (Material = null, Quantity = 0)
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "E-01", Quantity = 0, Material = null, Comment = "Prázdná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "E-02", Quantity = 0, Material = null, Comment = "Prázdná paleta" },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "E-03", Quantity = 0, Material = null },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "E-04", Quantity = 0, Material = null },
                new Pallet { Type = PalletType.Small, Status = PalletStatus.Stored, StorageLocation = "E-05", Quantity = 0, Material = null }
            };

            Pallets.AddRange(pallets);

            // 4. Kontrola validace přes DataAnnotations před samotným uložením
            var entitiesToValidate = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entitiesToValidate)
            {
                var validationContext = new ValidationContext(entity);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(entity, validationContext, validationResults, validateAllProperties: true))
                {
                    var errorMsg = validationResults.FirstOrDefault()?.ErrorMessage ?? "Chyba validace";
                    throw new InvalidOperationException($"Entita typu '{entity.GetType().Name}' není platná: {errorMsg}");
                }
            }

            // 5. Jediné finální uložení změn do databáze
            SaveChanges();
        }




    }
}

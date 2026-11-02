using DataEntity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows;

namespace Sklad;

public static class Globals
{
    public static WarehouseContext Context { get; private set; } = null!;

    public static void Initialize() => Context = new WarehouseContext();

    public static bool HasUnsavedChanges() =>
        Context.ChangeTracker.HasChanges();

    public static bool TrySaveData()
    {
        try
        {
            var errors = Context.ChangeTracker.Entries<BaseModel>()
                .Where(entry => entry.State is EntityState.Added
                    or EntityState.Modified)
                .SelectMany(entry => entry.Entity.GetValidationResults()
                    .Select(error => entry.Metadata.ClrType.Name + ": " +
                        error.ErrorMessage))
                .ToList();
            if (errors.Count != 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors),
                    "Chyba ověření", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                return false;
            }
            Context.SaveChanges();
            return true;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            return ReportError(exception,
                "Záznam mezitím změnil nebo odstranil jiný uživatel. " +
                "Změny nebyly uloženy. Zkopírujte si potřebné údaje a " +
                "použijte Storno / Vrátit změny pro nové načtení.");
        }
        catch (DbUpdateException exception)
        {
            return ReportError(exception,
                "Databáze změny nepřijala. Zkontrolujte vazby a připojení. " +
                "Změny zůstávají neuložené.");
        }
        catch (Exception exception)
        {
            return ReportError(exception,
                "Uložení se nezdařilo. Podrobnosti jsou v ladicím výstupu.");
        }
    }

    public static bool TryRollback()
    {
        // Zahození celé neuložené práce, nikoliv vrácení uložené transakce.
        WarehouseContext? replacement = null;
        try
        {
            replacement = new WarehouseContext();
            replacement.UnitsOfMeasure.Load();
            replacement.Materials.Load();
            replacement.Pallets.Load();
            var previous = Context;
            Context = replacement;
            previous.Dispose();
            return true;
        }
        catch (Exception exception)
        {
            replacement?.Dispose();
            return ReportError(exception,
                "Původní data nelze načíst. Okno ponechte otevřené " +
                "a zkontrolujte připojení.");
        }
    }

    private static bool ReportError(Exception exception, string message)
    {
        Debug.WriteLine(exception);
        MessageBox.Show(message, "Sklad", MessageBoxButton.OK,
            MessageBoxImage.Error);
        return false;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;

namespace DataEntity;

[AddINotifyPropertyChangedInterface]
[Table("Pallets")]
public class Pallet : BaseModel, IValidatableObject
{
    [Key]
    public int PalletId { get; set; }

    [EnumDataType(typeof(PalletType), ErrorMessage = "Neplatný typ palety.")]
    public PalletType Type { get; set; } = PalletType.Small;

    [EnumDataType(typeof(PalletStatus), ErrorMessage = "Neplatný stav palety.")]
    public PalletStatus Status { get; set; } = PalletStatus.Stored;

    [StringLength(10, ErrorMessage = "Adresa smí mít nejvýše 10 znaků.")]
    public string? StorageLocation { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Množství musí být nezáporné.")]
    public int Quantity { get; set; }

    public int? MaterialId { get; set; }

    [ForeignKey(nameof(MaterialId))]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public virtual Material? Material { get; set; }

    public string? Comment { get; set; }

    //private void OnMaterialChanged() => OnPropertyChanged(nameof(Quantity));
    //private void OnQuantityChanged() => OnPropertyChanged(nameof(Material));

    //ukázka validace, že pokud je Material null, Quantity musí být 0
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Material is null && Quantity != 0)
            yield return new ValidationResult(
                "Paleta bez materiálu musí mít nulové množství.",
                new[] { nameof(Material), nameof(Quantity) });
    }
}

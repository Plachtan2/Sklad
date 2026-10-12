using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity
{
    [Table("Materials")]
    public class Material : BaseModel
    {
        [Key]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Název materiálu je povinné pole")]
        [StringLength(100, ErrorMessage = "Jméno je omezeno na 100 znaků.")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Pojistná zásoba musí být nezáporné číslo.")]
        public int SafetyStock { get; set; } = 0;  // pojistná zásoba

        [Range(0, int.MaxValue, ErrorMessage = "Množství na paletu musí být nezáporné číslo.")]
        public int PalletCapacity { get; set; } = 0; // množství na paletu

        [Column(TypeName = "date")]
        public DateTime ReceivedOn { get; set; } = DateTime.Today; // datum přijetí materiálu

        public int UnitOfMeasureId { get; set; } // cizí klíč pro jednotku měření

        [ForeignKey(nameof(UnitOfMeasureId))]
        [Required(ErrorMessage = "Měrná jednotka je povinné pole")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; } = new UnitOfMeasure(); // navigační vlastnost pro jednotku měření



    }
}

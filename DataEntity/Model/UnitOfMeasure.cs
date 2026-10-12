using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity
{
    [AddINotifyPropertyChangedInterface]
    [Table("UnitsOfMeasure")]
    public class UnitOfMeasure : BaseModel
    {
        [Key]
        public int UnitOfMeasureId { get; set; }
        
        [Required (ErrorMessage = "Popis jednotky je povinné pole")]
        [StringLength(50, ErrorMessage = "Popis je omezen na 50 znaků.")]
        public string Description { get; set; } = "";

        public virtual ObservableCollection<Material> Materials { get; set; } = new (); // navigační vlastnost pro materiály

    }
}

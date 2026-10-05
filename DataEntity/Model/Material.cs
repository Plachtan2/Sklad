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
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required (ErrorMessage = "Název materiálu je povinné pole")]
        [StringLength(100, ErrorMessage = "Jméno je omezeno na 100 znaků.")]
        public string? Name { get; set; } 

    }
}

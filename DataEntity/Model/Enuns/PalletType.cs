using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity
{
    public enum PalletType
    {
        [Description("Malá")]
        Small = 0,
        [Description("Velká")]
        Large = 1,
        [Description("Dělená")]
        Split = 2        
    }
}

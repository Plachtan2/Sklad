using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace Sklad.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {

        public bool IsTablesEnabled { get; set; } = true;

    }
}

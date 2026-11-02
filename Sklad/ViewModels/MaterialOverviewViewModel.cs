using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntity;
using PropertyChanged;

namespace Sklad.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MaterialOverviewViewModel
    {

        private ObservableCollection<Material> materialsCol;
        public ObservableCollection<Material> MaterialsCol
        {
            get
            { 
              if (materialsCol == null)
                {
                    materialsCol = new ObservableCollection<Material>
                        (Globals.Context.Materials);                   
                }
                return materialsCol;
            }
        }

        public Material SelectedMaterial { get; set; } = new Material();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Sklad.Views;
using Sklad.Helpers;

namespace Sklad.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {

        public bool IsTablesEnabled { get; set; } = true;

        public ICommand OpenMaterialOverviewCommand { get; }

        public MainWindowViewModel()
        {
            OpenMaterialOverviewCommand = new RelayCommand(OpenMaterialOverview);
        }

        private void OpenMaterialOverview(object? parameter)
        {
            // Implement the logic to open the Material Overview window
            // For example, you can create a new instance of the MaterialOverviewWindow and show it
            var materialOverviewWindow = new MaterialOverviewWindow();
            materialOverviewWindow.Show();

          //  MessageBox.Show("Material Overview window opened! ");
        }

    }
}

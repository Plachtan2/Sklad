using DataEntity;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sklad.ViewModels
{
    public enum FormMode
    {
        View,
        Edit,
        Add
    }

    [AddINotifyPropertyChangedInterface]
    public class MaterialOverviewViewModel
    {

        private ObservableCollection<Material>? materialsCol;
        public ObservableCollection<Material> MaterialsCol
        {
            get
            { 
              if (materialsCol == null)
                {
                    materialsCol = new ObservableCollection<Material>
                        (Globals.Context.Materials);

                    // Vytvoření pohledu nad kolekcí pro účely filtrování
                    MaterialsView = CollectionViewSource.GetDefaultView(materialsCol);
                    MaterialsView.Filter = FilterMaterials;

                }
                return materialsCol;
            }
        }

        public Material SelectedMaterial { get; set; } = new Material();

        private ObservableCollection<UnitOfMeasure> unitsOfMeasureCol;
        public ObservableCollection<UnitOfMeasure> UnitsOfMeasureCol
        {
            get
            {
                if (unitsOfMeasureCol == null)
                {
                    unitsOfMeasureCol = new ObservableCollection<UnitOfMeasure>(Globals.Context.UnitsOfMeasure);
                }
                return unitsOfMeasureCol;
            }
        }


        //filtry

        public string SearchText { get; set; } = string.Empty;

        public ICollectionView MaterialsView { get; private set; }


        // Tuto metodu automaticky volá knihovna Fody při jakékoliv změně vlastnosti SearchText
        private void OnSearchTextChanged()
        {
            MaterialsView?.Refresh();
        }

        private bool FilterMaterials(object obj)
        {
            if (obj is Material mat)
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return true;

                if (string.IsNullOrWhiteSpace(mat.Name))
                    return false;

                // Využití CompareInfo pro hledání s ignorováním velikosti písmen a diakritiky
                CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                return compareInfo.IndexOf(mat.Name, SearchText, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
            }
            return false;
        }


        //STAVY

        // 1. Stavové vlastnosti
        public FormMode CurrentMode { get; set; } = FormMode.View;

        public bool IsViewMode => CurrentMode == FormMode.View;
        public bool IsEditMode => CurrentMode == FormMode.Edit || CurrentMode == FormMode.Add;

        public string FormHeader => CurrentMode switch
        {
            FormMode.Add => "PŘIDÁVÁNÍ",
            FormMode.Edit => "EDITACE",
            _ => "PROHLÍŽENÍ"
        };


    }
}

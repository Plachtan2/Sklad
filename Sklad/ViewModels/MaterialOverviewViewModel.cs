using DataEntity;
using PropertyChanged;
using Sklad.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
            set
            {
                materialsCol = value;
                MaterialsView = CollectionViewSource.GetDefaultView(materialsCol);
                MaterialsView.Filter = FilterMaterials;
            }
        }

        public Material SelectedMaterial { get; set; } 

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
            set
            {
                unitsOfMeasureCol = value;
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

        //AKCE - TLAČÍTKA

        // NOVÁ VLASTNOST pro řízení tlačítek Editovat a Smazat z UI (Fody automaticky notifikuje změny)
        public bool IsModifyEnabled => IsViewMode && SelectedMaterial != null;

        // Definice příkazů
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public MaterialOverviewViewModel()
        {
            // Inicializace příkazů - POUZE SaveCommand si ponechává CanExecute delegáta pro ukázku
            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            CancelCommand = new RelayCommand(ExecuteCancel);

            // UKÁZKA: Tlačítko OK je řízeno přes vnitřní logiku Commandu
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        // --- Logika pro PŘIDAT ---
        private void ExecuteAdd(object obj)
        {
            SelectedMaterial = new Material();
            CurrentMode = FormMode.Add;
        }

        // --- Logika pro EDITOVAT ---
        private void ExecuteEdit(object obj)
        {
            CurrentMode = FormMode.Edit;
        }

        // --- Logika pro SMAZAT ---
        private void ExecuteDelete(object obj)
        {
            if (SelectedMaterial == null) return; // Bezpečnostní pojistka

            if (SelectedMaterial.Pallets != null && SelectedMaterial.Pallets.Any())
            {
                MessageBox.Show("Materiál nelze smazat, protože k němu existují palety.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Opravdu chcete smazat materiál: {SelectedMaterial.Name}?", "Potvrzení", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Globals.Context.Materials.Remove(SelectedMaterial);
                if (Globals.TrySaveData())
                {
                    MaterialsCol.Remove(SelectedMaterial);
                    SelectedMaterial = MaterialsCol.FirstOrDefault();
                }
            }
        }

        // --- Logika pro OK (Uložit) ---
        private void ExecuteSave(object obj)
        {
            if (CurrentMode == FormMode.Add)
            {
                Globals.Context.Materials.Add(SelectedMaterial);
            }

            if (Globals.TrySaveData())
            {
                if (CurrentMode == FormMode.Add)
                {
                    MaterialsCol.Add(SelectedMaterial);
                }

                CurrentMode = FormMode.View;
                MaterialsView?.Refresh();
            }
        }

        // ZACHOVÁNO PRO UKÁZKU: Řídí, zda lze Command spustit (tlačítko OK v UI reaguje automaticky)
        private bool CanExecuteSave(object obj) => IsEditMode && SelectedMaterial != null && SelectedMaterial.IsValid;

        // --- Logika pro STORNO (Zrušit) ---
        private void ExecuteCancel(object obj)
        {
            Globals.TryRollback();
            ReloadData();
            CurrentMode = FormMode.View;
        }

        private void ReloadData()
        {
            MaterialsCol = new ObservableCollection<Material>(Globals.Context.Materials);
            UnitsOfMeasureCol = new ObservableCollection<UnitOfMeasure>(Globals.Context.UnitsOfMeasure);

            MaterialsView = CollectionViewSource.GetDefaultView(MaterialsCol);
            MaterialsView.Filter = FilterMaterials;

            SelectedMaterial = MaterialsCol.FirstOrDefault();
        }

        // ... (ponech stávající metody OnSearchTextChanged a FilterMaterials)
    }



}


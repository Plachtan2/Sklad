using DataEntity;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sklad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



            var context = new WarehouseContext();
            
            //context.ClearAndReseed();

            //context.Seed(); // Seed the database with initial data if necessary

            //AddSampleMaterialWithUoM();
            //ShowMaterials();

        }

        void AddSampleMaterial()
        {
            var context = new WarehouseContext();
            var newMaterial = new Material
            {
                Name = "Cihla",
                SafetyStock = 1000,
                PalletCapacity = 50
            };
            context.Materials.Add(newMaterial);
            context.SaveChanges();
        }

        void AddSampleMaterialWithUoM()
        {
            var context = new WarehouseContext();

            var uom = context.UnitsOfMeasure.FirstOrDefault(u => u.Description == "ks");

            if (uom!=null)
            {
                var newMaterial = new Material
                {
                    Name = "Cihla2",
                    SafetyStock = 1000,
                    PalletCapacity = 50,
                    UnitOfMeasure = uom
                };
                context.Materials.Add(newMaterial);
                context.SaveChanges();
            }

        }


        private void ShowMaterials()
        {
            var context = new WarehouseContext();

            //var material = context.Materials.Where(m => m.SafetyStock > 1200).FirstOrDefault();

            //if (material != null)
            //{
            //    MessageBox.Show($"Material: {material.Name}, Safety Stock: {material.SafetyStock}, Pallet Capacity: {material.PalletCapacity}");
            //}
            //else
            //{
            //    MessageBox.Show("Nebyl nalezen mat dle kritéria.");
            //}

            var materials = context.Materials.ToList();

            string message = string.Join("\n", materials.Select(m => $"Material: {m.Name}, Safety Stock: {m.SafetyStock}, Pallet Capacity: {m.PalletCapacity}"));

            MessageBox.Show(message);

        }


    }
}
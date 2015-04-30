using System.Windows;
using Omu.ValueInjecter;

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var p = new Product()
            {
                ModelName = "das name",
                Description = "bla",
                ModelNumber = "über",
                UnitCost = 1231
            };
            var vm = new ProductViewModel();
            gridProductDetails.DataContext = vm;

            vm.InjectFrom(p);
        }
    }
}
